-- creating roles table
create table Roles (
	id nvarchar(450) primary key,
	roleName varchar(10) not null
)

-- creating BerthClass table
create table BerthClass (
	id nvarchar(450) primary key,
	class char(2) not null,
	pricePerKm float not null
)

-- creating BerthStatus table
create table BerthStatus (
	id nvarchar(450) primary key,
	statusName varchar(11) not null,
)

-- creating TicketStatus table
create table TicketStatus (
	id nvarchar(450) primary key,
	statusName varchar(20) not null
)

-- creating TrainStatus table
create table TrainStatus (
	id nvarchar(450) primary key,
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
	id nvarchar(450) primary key,
	userPassword nvarchar(255) not null,
	salt nvarchar(255) not null
)

-- creating Users table
create table Users (
	id nvarchar(450) primary key,
	firstName varchar(30) not null,
	lastName varchar(30),
	email nvarchar(MAX) not null,
	userPassword nvarchar(450) references Passwords(id) on update cascade on delete cascade,
	userRole nvarchar(450) references Roles(id) on update cascade on delete cascade,
	phoneNumber char(10) not null
)

-- creating Trains table
create table Trains (
	trainNo int primary key,
	trainName varchar(30),
	trainStatus nvarchar(450) references TrainStatus(id) on update cascade on delete cascade,
	availableSeatsInA1 tinyint not null,
	availableSeatsInA2 tinyint not null,
	availableSeatsInA3 tinyint not null,
	totalSeatsInA1 tinyint not null,
	totalSeatsInA2 tinyint not null,
	totalSeatsInA3 tinyint not null
)

-- creating Distance table
create table Distance (
	id nvarchar(450) primary key,
	boardingStation varchar(50) not null,
	destination varchar(50) not null,
	distance float not null,
	train int references Trains(trainNo) on update cascade on delete cascade
)

-- creating Tickets table
create table Tickets (
	pnrNo int primary key,
	passengerId nvarchar(450) references Users(id) on update cascade on delete cascade,
	trainNo int references Trains(trainNo) on update cascade on delete cascade,
	berthNo tinyint not null,
	berthClass char(2) not null,
	price float not null,
	ticketStatus nvarchar(450) references ticketStatus(id) on update cascade on delete cascade,
	bookingDate datetime not null
)

-- creating TriggerLogging table
create table TriggerLogging (
	id int primary key identity,
	triggerName varchar(100) not null,
	triggerType int references TriggerType(id),
	queryType int references QueryType(id),
	executionStartTime datetime not null,
	executionEndTime datetime not null,
	triggerStatus int references TriggerStatus(id)
)

insert into TriggerType values ('AFTER'), ('INSTEAD OF'), ('BEFORE')
insert into QueryType values ('INSERT'), ('UPDATE'), ('DELETE')
insert into TriggerStatus values ('SUCCESSFULL'), ('FAILED')
