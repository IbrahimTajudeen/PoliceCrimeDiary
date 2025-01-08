------------------------------------------------------------------------------------------------------------
----------------- ####################### DROPING Database Schema ####################### ------------------
------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('Inbox') IS NOT NULL
	DROP TABLE Inbox;

IF OBJECT_ID('BailList') IS NOT NULL
	DROP TABLE BailList;

IF OBJECT_ID('ChargedCrimes') IS NOT NULL
	DROP TABLE ChargedCrimes;

IF OBJECT_ID('Transfers') IS NOT NULL
	DROP TABLE Transfers;

IF OBJECT_ID('CrimeReport') IS NOT NULL
	DROP TABLE CrimeReport;

IF OBJECT_ID('Users') IS NOT NULL
	DROP TABLE Users;


------------------------------------------------------------------------------------------------------------
----------------- ####################### Creating Database Schema ####################### -----------------
------------------------------------------------------------------------------------------------------------

CREATE TABLE Users(ID INT UNIQUE IDENTITY(1,1) NOT NULL, 
					Name VARCHAR(50) NOT NULL,
					Sex VARCHAR(50) NOT NULL CHECK(LOWER(Sex) IN('male', 'female')),
					Address VARCHAR(MAX) NOT NULL,
					Email VARCHAR(MAX) NOT NULL,
					Phone VARCHAR(50) NOT NULL,
					Picture NVARCHAR(50),
					ModeOfID VARCHAR(MAX) NOT NULL,
					PCID INT NOT NULL,
					JoinDate DATE DEFAULT(SYSDATETIME()),
					Password NVARCHAR(MAX) NOT NULL,
					Username NVARCHAR(MAX) NOT NULL,
					UserType VARCHAR(50) CHECK(LOWER(UserType) IN ('user', 'police', 'admin')) NOT NULL);


CREATE TABLE CrimeReport(ID INT UNIQUE IDENTITY(1,1) NOT NULL,
						ReportedBy INT FOREIGN KEY REFERENCES Users(ID),
						Name VARCHAR(50) NOT NULL,
						Sex VARCHAR(10) CHECK(LOWER(Sex) IN('male', 'female')) NOT NULL,
						CrimeType VARCHAR(50) NOT NULL,
						Evidence VARCHAR(50) NOT NULL,
						DateofCrime DATE NOT NULL DEFAULT(SYSDATETIME()),
						TimeOfCrime TIME NOT NULL,
						Description VARCHAR(MAX),
						Picture VARCHAR(50),
						RelatedDocuments VARCHAR(MAX),
						Location VARCHAR(50) NOT NULL,
						DateReported DATE DEFAULT(SYSDATETIME()),
						CaseTakenBy INT DEFAULT(0),
						Status VARCHAR(50) DEFAULT('Reported'));

CREATE TABLE Transfers(ID INT UNIQUE IDENTITY(1,1) NOT NULL,
					   CrimeID INT FOREIGN KEY REFERENCES CrimeReport(ID),
					   TransferFrom VARCHAR(100) NOT NULL,
					   TransferTo VARCHAR(100) NOT NULL,
					   TransferDate DATE NOT NULL DEFAULT(SYSDATETIME()),
					   Reason VARCHAR(MAX));


CREATE TABLE ChargedCrimes(ID INT UNIQUE IDENTITY(1,1) NOT NULL,
					   CrimeID INT FOREIGN KEY REFERENCES CrimeReport(ID),
					   OfficerInCharge INT FOREIGN KEY REFERENCES Users(ID) NOT NULL,
					   ChargeDetails VARCHAR(100) NOT NULL,
					   ChargeDate DATE NOT NULL DEFAULT(SYSDATETIME()),
					   ChargeEvidence NVARCHAR(MAX),
					   CourtName VARCHAR(50) NOT NULL,
					   CourtDate DATE NOT NULL,
					   Status VARCHAR(50) DEFAULT('In Court'));


CREATE TABLE BailList(ID INT UNIQUE IDENTITY(1,1) NOT NULL,
					  SuspectID INT FOREIGN KEY REFERENCES CrimeReport(ID),
					  OfficerInCharge INT FOREIGN KEY REFERENCES Users(ID),
					  BailAmount VARCHAR(MAX) NOT NULL,
					  AmountOffered VARCHAR(MAX) DEFAULT('0.00'),
					  Status VARCHAR(50) DEFAULT('Bail Allow'));

CREATE TABLE Inbox(ID INT UNIQUE IDENTITY(1,1) NOT NULL,
				   SenderID INT FOREIGN KEY REFERENCES Users(ID),
				   PostMessage VARCHAR(MAX),
				   PostDate Date DEFAULT(SYSDATETIME()),
				   PostTime Time DEFAULT(SYSDATETIME()),
				   IsSecret VARCHAR(50) DEFAULT(NULL),
				   OnlyBy VARCHAR(50) DEFAULT(NULL));




------------------------------------------------------------------------------------------------------------
------------ ####################### Inserting to Database Table Schema ####################### ------------
------------------------------------------------------------------------------------------------------------
