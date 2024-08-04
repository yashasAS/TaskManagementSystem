Task Management System

Overview
This Task Management System is designed to help employees, managers, team leaders, and company administrators effectively track and manage tasks. The system provides role-based authentication and authorization using JWT tokens, allowing different levels of access and functionality based on user roles.

Features

1.Role-Based Authentication and Authorization
   ->Implemented using JWT tokens.
   ->Different roles include Employee, Manager, Team Lead, MD, and Admin. (Created while inserting rfecord in Users Table)
   ->Employee Task Management

2.Employees can track their tasks.
   ->Employees can attach documents and notes to their tasks.
   ->Employees can mark tasks as complete.
   ->Manager and Team Lead Task Tracking

3.Managers and Team Leads can track tasks assigned to their respective teams.
   ->Ability to view tasks by team members.
   ->Admin and MD Reporting

4.Admins and MDs can generate task reports.
   ->Reports include tasks with due dates

API End Points
  
1.LoginController
  All user needs to get the Jwt token to access the system, the Jwt token has the user role encoded
  
2.TaskController
  This controller has basic CRUD operations by Id, duedate etc and attach documents and files for the tasks
  
3.UserController 
  This controller does not have permission for employee, to access this controller the Jwt token should have "Admin" or "MD" as a role defined it
  
4.NotesController
  This has basic CRUD operations to maintain notes

5.DocumentController
  This has basic CRUD operations to maintain documents

6.ReportAndStatzController
    This controller does not have permission for employee, to access this controller the Jwt token should have "Admin" or "MD" or "Manager" as a role defined it
    This has a set of APIs that generate report by team, user and custom date , so that user can easily track individual or teams progress

Prerequisites are .NET SDK, SQL Server and Install following packages in visualstudio
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.Extensions.Configuration
Microsoft.VisualStudio.Web.CodeGeneration.Design
