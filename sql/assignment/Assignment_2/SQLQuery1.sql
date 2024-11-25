use Assignments

-- creating departments table
create table Departments(
	departmentNo int primary key,
	departmentName varchar(15),
	loc varchar(10)
)

-- creating employees table
create table Employees(
	employeeNo int primary key,
	employeeName varchar(10),
	job varchar(10),
	managerId int,
	hireDate date,
	salary int,
	comm int,
	deptNo int references Departments(departmentNo)
)

-- inserting into departments table
insert into Departments values(10, 'ACCOUNTING', 'NEW YORK'),
(20, 'RESEARCH', 'DALLAS'), (30, 'SALES', 'CHICAGO'), (40, 'OPERATIONS', 'BOSTON')

-- inserting into employees table
insert into Employees values(7369, 'SMITH', 'CLERK', 7902, '1980-12-17', 800, null, 20),
(7499, 'ALLEN', 'SALESMAN', 7698, '1981-02-20', 1600, 300, 30),
(7521, 'WARD', 'SALESMAN', 7698, '1981-02-22', 1250, 500, 30),
(7566, 'JONES', 'MANAGER', 7839, '1981-04-02', 2975, null, 20),
(7654, 'MARTIN', 'SALESMAN', 7698, '1981-09-28', 1250, 1400, 30),
(7698, 'BLAKE', 'MANAGER', 7839, '1981-05-01', 2850, null, 30),
(7782, 'CLARK', 'MANAGER', 7839, '1981-06-09', 2450, null, 10),
(7788, 'SCOTT', 'ANALYST', 7566, '1987-04-19', 3000, null, 20),
(7839, 'KING', 'PRESIDENT', null, '1981-11-17', 5000, null, 10),
(7844, 'TURNER', 'SALESMAN', 7698, '1981-09-08', 1500, 0, 30),
(7876, 'ADAMS', 'CLERK', 7788, '1987-05-23', 1100, null, 20),
(7900, 'JAMES', 'CLERK', 7698, '1981-12-03', 950, null, 30),
(7902, 'FORD', 'ANALYST', 7566, '1981-12-03', 3000, null, 20),
(7934, 'MILLER', 'CLERK', 7782, '1982-01-23', 1300, null, 10)

-- query to display employees whose name starts with 'A'
select employeeName from Employees where employeeName like 'A%'

-- query to select all employees who don't have manager
select * from Employees where managerId is null

-- query to display employee name, number and salary who earn in range 1200 to 1400
select employeeName, employeeNo, salary from Employees where salary >= 1200 and salary <= 1400

-- 4. Give all the employees in the RESEARCH department a 10% pay rise. Verify that this has been done by listing all their details before and after the rise.
select * from Employees where deptNo = (select departmentNo from Departments where departmentName = 'RESEARCH')

update Employees set salary = salary * 1.10 where deptNo = (select departmentNo from Departments where departmentName = 'RESEARCH')

select * from Employees where deptNo = (select departmentNo from Departments where departmentName = 'RESEARCH')

-- 5. Find the number of CLERKS employed. Give it a descriptive heading.
select COUNT(*) as numberOfClerks from Employees where job = 'CLERK'

-- 6. Find the average salary for each job type and the number of people employed in each job.
select job, AVG(salary) as averageSalary, COUNT(*) as noOfEmployees from Employees group by job

-- 7. List the employees with the lowest and highest salary.
select * from Employees where salary = (SELECT MIN(salary) FROM Employees) or salary = (SELECT MAX(salary) FROM Employees)

-- 8. List full details of departments that don't have any employees.
select * from Departments where departmentNo not in (select distinct deptNo from Employees)

-- 9. Get the names and salaries of all the analysts earning more than 1200 who are based in department 20. Sort the answer by ascending order of name.
select employeeName, salary from Employees where job = 'ANALYST' and salary > 1200 and deptNo = 20 order by employeeName

-- 10. For each department, list its name and number together with the total salary paid to employees in that department.
select D.departmentName, D.departmentNo, SUM(E.salary) as totalSalary from Departments D join Employees E on D.departmentNo = E.deptNo group by D.departmentName, D.departmentNo

-- 11. Find out salary of both MILLER and SMITH.
select salary from Employees where employeeName in ('MILLER', 'SMITH')

-- 12. Find out the names of the employees whose name begin with ‘A’ or ‘M’.
select employeeName from Employees where employeeName like 'A%' or employeeName like 'M%'

-- 13. Compute yearly salary of SMITH. 
select employeeName, salary * 12 as yearlySalary from Employees where employeeName = 'SMITH'

-- 14. List the name and salary for all employees whose salary is not in the range of 1500 and 2850. 
select employeeName, salary from Employees where salary not between 1500 and 2859

-- 15. Find all managers who have more than 2 employees reporting to them
select managerId from Employees group by managerId having COUNT(employeeNo) > 2;