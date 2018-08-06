import requests
import time

class DataStream_UCTS:
    
    def __init__(self,user_id,password ):
        """
        Arguments:
            user_id {string} -- DataStream username
            password {string} -- DataStream password
        """
        self.server_url = 'http://product.datastream.com'
        self.UCTS_page_address = '/UCTS/UCTSMaint.asp'
        self.datastream_user_id = user_id
        self.datastream_password = password
        
    def convert_data_to_string(self, data ):
        """
        Convert dictionary data values to string, separated by commas. 
        Datastream follows weekdays only for daily data, adjust the input data as this function does not skip weeeknds
        
        Arguments:
            data {Dictionary} -- Date & Values

        """
       
        for k, v in data.items():
            if not v.isnumeric :
                data[k] = 'NAVALUE'
        val_string = ','.join(data.values())
        return val_string
    
    def get_UCTS_url (self):
        """
        Create UCTS Url, specific to the Username

        """
        
        return self.server_url + self.UCTS_page_address + "?UserId="+ self.datastream_user_id
        
    
    def encode_password(self):
        """
        Encoding Password
        
        """
        encoded_password=""
        seed=199
        for c in self.datastream_password:
            asc_byte= ord(c)
            crypted_byte = asc_byte ^ seed
            encoded_password +=str(crypted_byte).zfill(3)
            seed = ((seed + crypted_byte ) % 255) ^ 67
        return encoded_password
    
 
    def upload_UCTS(self,
                    data,
                    ds_code,
                    management_group,
                    start_date,
                    end_date,
                    frequency,
                    title,
                    units,
                    dec_places,
                    as_perc,
                    freq_conv,
                    alignment,
                    carry_ind,
                    prime_curr  
                    ):
        """
        UCTS full upload, OVERWRIITE UCTS if exists without confirmation

        Arguments:
            data {Dictionary} -- Date & Values
            ds_code {String} -- 8 Digit Code
            management_group {String} -- Management Group
            start_date {String} -- Start date, only accept d/m/yyyy format
            end_date {String} -- End date, only accept d/m/yyyy format
            frequency {String} -- frequency - Possible values D,W,M,Q,Y
            title {String} --  Title
            units {String} -- Unit
            dec_places {String} -- Number of decimal places- as string
            as_perc {String} -- As % - Possible values N,Y 
            freq_conv {String} -- Frequency Conversion - Possible value ACT,SUM,AVG,END
            alignment {String} -- Alignment - Possible values 1ST,MID,END
            carry_ind {String} -- Carry Indicator - Possible values YES,NO,PAD
            prime_curr {String} -- urrency - Use DataStream to find currency code

        Returns:
            {String} -- *OK* if success, or error if any
        """

        parameters = {         
               
                    "CallType": "Upload" ,
                    "TSMnemonic": ds_code.upper(),
                    "TSMLM": management_group,
                    "TSStartDate":start_date,
                    "TSEndDate":end_date,
                    "TSFrequency":frequency,
                    "TSTitle": title,
                    "TSUnits":units,
                    "TSDecPlaces":dec_places,
                    "TSAsPerc":as_perc,
                    "TSFreqConv":freq_conv,                 
                    "TSAlignment":alignment,
                    "TSCarryInd":carry_ind,
                    "TSPrimeCurr":prime_curr,
                    "TSULCurr":"",
                    "ForceUpdateFlag1":"Y",
                    "ForceUpdateFlag2":"Y",
                    "TSValsStart":start_date,
                    "TSValues": self.convert_data_to_string(data),
                    "UserOption": self.encode_password()
                    }
        
        
        attempts=0
        result_text =""

        #re-submit after 2 seconds if fails, max attempt 2
        while attempts < 2 and result_text != '*OK*' :
            result =requests.post(self.get_UCTS_url (),parameters)
            result_text =  result.text           
            if result_text  == '*OK*':
                break
            
            time.sleep(2)
            attempts+=1
       
        return  result_text

