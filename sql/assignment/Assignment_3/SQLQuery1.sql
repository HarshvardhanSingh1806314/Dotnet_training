use Assignments

-- 1. Retrieving list of all managers
select managerId from Employees

-- 2. displaying names of all employees earning more than 1000 per month
select employeeName, salary from Employees where (salary/12) > 1000

-- 3. displaying names and salaries of all employees except JAMES
select employeeName, salary from Employees where employeeName not in ('JAMES')

-- 4. displaying details of all employees whose name begin with 'S'
select * from Employees where employeeName like 'S%'

-- 5. displaying names of all employees that have 'A' anywhere in their name
select * from Employees where employeeName like '%A%'

-- 6. displaying names of all employees that have 'L' as their third character in their name
select * from Employees where employeeName like '__L%'

-- 7. computing daily salary of JONES
select (salary / 30) as monthlySalary from Employees where employeeName = 'JONES'

-- 8. calculating the total monthly salary of all employees
select SUM(salary) as totalMonthlySalary from Employees

-- 9. printing avg annual salary
declare @averageAnnualSalary float 
select @averageAnnualSalary = SUM(salary) / 12 from Employees
print 'Average Annual Salary: ' + cast(@averageAnnualSalary as varchar(20))

-- 10. Select the name, job, salary, department number of all employees except SALESMAN from department number 30.
select employeeName, job, salary, deptNo from Employees except select employeeName, job, salary, deptNo from Employees where job = 'SALESMAN' and deptNo = '30'

-- 11. List unique departments of the EMP table.
select distinct deptNo from Employees

-- 12. List the name and salary of employees who earn more than 1500 and are in department 10 or 30
select employeeName, salary from Employees where salary > 1500 and deptNo in (10, 30)

-- 13.  Display the name, job, and salary of all the employees whose job is MANAGER or ANALYST and their salary is not equal to 1000, 3000, or 5000.
select employeeName, job, salary from Employees where job in ('MANAGER', 'ANALYST') and salary not in (1000, 3000, 5000)

-- 14. Display the name, salary and commission for all employees whose commission amount is greater than their salary increased by 10%.
select employeeName, salary, comm from Employees where comm > salary * 1.10

-- 15. Display the name of all employees who have two Ls in their name and are in department 30 or their manager is 7782.
select employeeName from Employees where employeeName like '%LL%' and (deptNo = 30 or managerId = 7782)

-- 16. Display the name of all employees who have two Ls in their name and are in department 30 or their manager is 7782.
select employeeName from Employees where DATEDIFF(year, hireDate, GETDATE()) between 30 and 40
select COUNT(*) from Employees

-- 17. Retrieve the names of departments in ascending order and their employees in descending order.
select employeeName, departmentName from Employees e join Departments d on e.deptNo = d.departmentNo order by e.employeeName desc, d.departmentName asc

-- 18. Find out experience of MILLER.
select DATEDIFF(YEAR, hireDate, GETDATE()) as experience from Employees where employeeName = 'MILLER'