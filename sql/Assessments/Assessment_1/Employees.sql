use Assessment

-- creating employee table
create table Employee(
	id int primary key,
	employeeName varchar(10),
	age int,
	addrss varchar(10),
	salary int
)

-- inserting into employee table
insert into Employee values(1, 'Ramesh', 32, 'Ahmedabad', 2000),
(2, 'Khilan', 25, 'Delhi', 1500), (3, 'kaushik', 23, 'kota', 2000),
(4, 'Chaitali', 25, 'Mumbai', 6500), (5, 'Hardik', 27, 'Bhopal', 8500),
(6, 'Komal', 22, 'MP', null), (7, 'Muffy', 24, 'Indore', null)

-- query to display name of employees in lower case whose salary is null
select employeeName from Employee where salary is null