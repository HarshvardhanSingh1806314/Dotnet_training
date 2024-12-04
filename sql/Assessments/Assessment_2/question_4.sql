use Assignments

-- question 4
create table Employee(
	--employeeNo int primary key,
	--employeeName varchar(30),
	--employeeSalary decimal(10, 2),
	--employeeDateOfJoining date
)

begin transaction

-- inserting 3 rows in the table employee
insert into Employee values (1, 'Harshvardhan Singh', 5000.00, '2020-01-01'),
(2, 'Devanshu Bhargava', 5050.00, '2021-03-21'), (3, 'Aadi Jain', 4050.00, '2022-05-14')

-- updating second row salary by 15%
update Employee set employeeSalary = employeeSalary * 1.15 where employeeNo = 2

-- deleting first row
declare @deletedRow table(employeeNo int , employeeName varchar(30), employeeSalary decimal(10, 2), employeeDateOfJoining date)

-- saving the deleted row in a temporary table
delete from Employee output deleted.employeeNo, deleted.employeeName, deleted.employeeSalary, deleted.employeeDateOfJoining into @deletedRow where employeeNo = 1

-- recalling the deleted row without losing the increment of second row
insert into Employee(employeeNo, employeeName, employeeSalary, employeeDateOfJoining)
select employeeNo, employeeName, employeeSalary, employeeDateOfJoining from @deletedRow

-- commiting the transaction
commit
