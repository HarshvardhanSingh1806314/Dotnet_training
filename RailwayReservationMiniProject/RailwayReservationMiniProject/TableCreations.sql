-- creating roles table
create table Roles (
	id int primary key identity,
	roleName varchar(10) not null
)

-- creating BerthClass table
create table BerthClass (
	id int primary key identity,
	class char(2) not null,
	pricePerKm float not null
)

-- creating BerthStatus table
create table BerthStatus (
	id int primary key identity,
	statusName varchar(11) not null,
)

-- creating TicketStatus table
create table TicketStatus (
	id int primary key identity,
	statusName varchar(20) not null
)

-- creating TrainStatus table
create table TrainStatus (
	id int primary key identity,
	statusName varchar(11) not null
)

-- creating TriggerStatus table
create table TriggerStatus (
	id int primary key identity,
	statusName varchar(20) not null
)

-- creating TriggerType table
create table TriggerType (
	id int primary key identity,
	triggerType varchar(15) not null
)

-- creating QueryType table
create table QueryType (
	id int primary key identity,
	queryType varchar(10) not null
)

-- creating Passwords table
create table Passwords (
	id int primary key identity,
	userPassword nvarchar(255) not null,
	salt nvarchar(255) not null
)

-- creating Users table
create table Users (
	id int primary key,
	firstName varchar(30) not null,
	lastName varchar(30),
	email nvarchar(30) not null,
	userPassword int references Passwords(id),
	userRole int references Roles(id),
	phoneNumber nvarchar(20) not null
)

-- creating Trains table
create table Trains (
	trainNo int primary key,
	trainName varchar(30),
	trainStatus int references TrainStatus(id),
	availableSeatsInA1 tinyint not null,
	availableSeatsInA2 tinyint not null,
	availableSeatsInA3 tinyint not null,
	totalSeatsInA1 tinyint not null,
	totalSeatsInA2 tinyint not null,
	totalSeatsInA3 tinyint not null
)

-- creating Berth table
create table Berth (
	berthNo int not null,
	berthClass int references BerthClass(id),
	trainNo int references Trains(trainNo),
	berthStatus int references BerthStatus(id),
)

-- creating Distance table
create table Distance (
	id int primary key identity,
	boardingStation varchar(30) not null,
	destination varchar(30) not null,
	distance float not null
)

-- creating Tickets table
create table Tickets (
	pnrNo int primary key,
	passengerId int references Users(id),
	trainNo int references Trains(trainNo),
	berthNo tinyint not null,
	price float not null,
	ticketStatus int references ticketStatus(id),
	boardingDate datetime not null,
	bookingDate datetime not null
)

-- creating TriggerLogging table
create table TriggerLogging (
	id int primary key identity,
	triggerName varchar(30) not null,
	triggerType int references TriggerType(id),
	queryType int references QueryType(id),
	executionStartTime datetime not null,
	executionEndTime datetime not null
)