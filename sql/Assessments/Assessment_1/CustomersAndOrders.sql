use Assessment

-- creating customers table
create table Customers(
	id int primary key,
	employeeName varchar(10),
	age int,
	addrss varchar(10),
	salary int
)

-- creating orders table
create table Orders(
	orderId int primary key,
	orderDate datetime,
	customerId int references Customers(id)
)

-- inserting into customers table
insert into Customers values(1, 'Ramesh', 32, 'Ahmedabad', 2000),
(2, 'Khilan', 25, 'Delhi', 1500), (3, 'kaushik', 23, 'Kota', 2000),
(4, 'Chaitali', 25, 'Mumbai', 6500), (5, 'Hardik', 27, 'Bhopal', 8500),
(6, 'Komal', 22, 'MP', 4500), (7, 'Muffy', 24, 'Indore', 10000)

-- inserting into orders table
insert into Orders values(102, '2009-10-08 00:00:00', 3, 3000),
(100, '2009-10-08 00:00:00', 3, 1500), (101, '2009-11-20 00:00:00', 2, 1560),
(103, '2008-05-20 00:00:00', 4, 2060)

-- displaying name of customer who has '0' anywhere in address
select addrss from Customers where addrss like '%o%'

-- displaying date, total no of customer placed order on same date
select orderDate, COUNT(customerId) from Orders group by orderDate
