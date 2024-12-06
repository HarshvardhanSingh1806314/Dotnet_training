use Assignments
drop procedure sp_calculatePaySlip
create or alter proc sp_calculatePayslip @employeeId int
as
begin
	declare @hra decimal(10, 2)
	declare @da decimal(10, 2)
	declare @pf decimal(10, 2)
	declare @it decimal(10, 2)
	declare @deductions decimal(10, 2)
	declare @grossSalary decimal(10, 2)
	declare @netSalary decimal(10, 2)
	declare @salary int
	declare @employeeName varchar(10)

	select @employeeName = employeeName from Employees where employeeNo = @employeeId

	select @salary = salary from Employees where employeeNo = @employeeId
	set @hra = @salary * 0.10
	set @da = @salary * 0.20
	set @pf = @salary * 0.08
	set @it = @salary * 0.05
	set @deductions = @pf + @it
	set @grossSalary = @salary + @hra + @da
	set @netSalary = @grossSalary - @deductions

	PRINT '--------------------------------------------------';
    PRINT '                   PAYSLIP                        ';
    PRINT '--------------------------------------------------';
    PRINT 'Employee Name   : ' + @employeeName;
    PRINT 'Basic Salary    : ' + CAST(@salary as varchar(20));
    PRINT '--------------------------------------------------';
    PRINT 'HRA (10% of Salary)   : ' + CAST(@hra as varchar(20));
    PRINT 'DA (20% of Salary)    : ' + CAST(@da as varchar(20));
    PRINT 'PF (8% of Salary)     : ' + CAST(@pf as varchar(20));
    PRINT 'IT (5% of Salary)     : ' + CAST(@it as varchar(20));
    PRINT '--------------------------------------------------';
    PRINT 'Deductions (PF + IT) : ' + CAST(@deductions as varchar(20));
    PRINT '--------------------------------------------------';
    PRINT 'Gross Salary         : ' + CAST(@grossSalary as varchar(20));
    PRINT 'Net Salary           : ' + CAST(@netSalary as varchar(20));
    PRINT '--------------------------------------------------';
end

exec sp_calculatePayslip 7839