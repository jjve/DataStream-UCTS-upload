
# DataStream UCTS upload
C# & Python

A class provides functionality to upload  user created time series (UCTS) to the Thomson Reuters DataStream server. You can use these series in Datastream, DataStream charting and other reports.

```diff
-This class function will overwrite any existing UCTS series without asking for confirmation. 
-It does a full upload of data.
```


## Getting Started

Copy the class to your project, create  an instance with your DataStream username and password.

C#

```
DataStream_UCTS UCTS = new DataStream_UCTS("YOUR DATASTREAM USERID", "YOUR DATASTREAM PASSWORD");
```


Python

```
UCTS= DataStream_UCTS("YOUR DATASTREAM USERID", "YOUR DATASTREAM PASSWORD")
```


DataStream accept dates only in d/m/yyyy format.


### Daily series upload 

DataStream accept weekdays only series, please skip weekends from your input data.

C#

```
Dictionary<string, string> DataDaily = new Dictionary<string, string>();
DataDaily.Add ("1/8/2018","188");
DataDaily.Add("2/8/2018", "100.10");
DataDaily.Add("3/8/2018", "100.15");
await UCTS.UploadUCTS(DataDaily, "TSJJJJDD", "TEST", "1/8/2018", "3/8/2018", "D", "", "", "2", "N", "END", "END", "YES", "E"); 
```

Python

```

data_daily={'1/8/2018':'100.12','2/8/2018':'1004.10','3/8/2018':'107.15'}
res=UCTS.upload_UCTS(data_daily, 'TSJJJJDD', 'TEST', '1/8/2018', '3/8/2018', 'D', '', '', '2','N', 'END', 'END', 'YES', 'E') 

```

### Montly series upload 

You should adjust the start and end date according to the alignment parameter

1ST – please use month  start as  start date in 1/m/yyyy format and end date as 1/m/yyyy.

MID - please use mid month as  start date in 16/m/yyyy format and end date as 16/m/yyyy.

END - please use month end as  start date in  30 or 31/m/yyyy format and end date as 31 OR 30/m/yyyy.


C#

```
Dictionary<string, string> DataMonthly = new Dictionary<string, string>();

DataMonthly.Add("1/6/2018", "100");
DataMonthly.Add("1/7/2018", "102.12");
DataMonthly.Add("1/8/2018", "103.25");
await UCTS.UploadUCTS(DataMonthly, "TSJJJJDM", "TEST", "30/6/2018", "31/8/2018", "M", "", "", "2", "N", "END", "END", "YES", "E");
            
 ```
 
Python

```

data_monthly={'1/6/2018':'100.4','1/7/2018':'104.10','1/8/2018':'109.15'}
res=UCTS.upload_UCTS(data_monthly, 'TSJJJJDM', 'TEST', '30/6/2018', '31/8/2018', 'M', '', '', '2','N', 'END', 'END', 'YES', 'E') 

```

### Quarterly series upload 

Please use quarter number and year format as date column, such as Q1 2018

C#
```
Dictionary<string, string> DataQuarterly = new Dictionary<string, string>();

DataQuarterly.Add("Q1 2018", "100");
DataQuarterly.Add("Q2 2018", "103.10");
DataQuarterly.Add("Q3 2018", "105.15");
await UCTS.UploadUCTS(DataQuarterly, "TSJJJJDQ", "TEST", "31/3/2018", "30/9/2018", "Q", "", "", "2", "N", "END", "END", "YES", "E");
```

Python

```
data_quarterly={'Q1 2018':'103.2','Q2 2018':'104.10','Q3 2018':'112.15'}
res=UCTS.upload_UCTS(data_quarterly, 'TSJJJJDQ', 'TEST', '31/3/2018', '30/9/2018', 'Q', '', '', '2','N', 'END', 'END', 'YES', 'E')

```
### Yearly series upload 

Please use yearas date column, 2018

use start date 1/1/yyyy for Date Alignment 1ST and 31/m/yyyy for END

C#

```
Dictionary<string, string> DataYearly = new Dictionary<string, string>();
DataYearly.Add("2016", "100");
DataYearly.Add("2017", "106.11");
DataYearly.Add("2018", "1010.35");            
await UCTS.UploadUCTS(DataYearly, "TSJJJJDY", "TEST", "31/12/2016", "31/12/2018", "Y", "", "", "2", "N", "END", "END", "YES", "E");
```

Python

```
data_yearly={'2016':'103.2','2017':'107.10','2018':'118.15'}
res=UCTS.upload_UCTS(data_yearly, 'TSJJJJDY', 'TEST', '31/12/2016', '31/12/2018', 'Y', '', '', '2','N', 'END', 'END', 'YES', 'E')

```
