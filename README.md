### Daily Routine Todo App

This is a windows todo app that allows you to add, edit, view and delete daily tasks. 
The aim of the app is to help you save and track all your daily tasks in one place. Even for years to come, 
you can always look back and see what you did on a particular day. You can also view each category of tasks in a year graphically.

### Features
- Add a task
- Edit a task
- Delete a task
- View all tasks
- View tasks by category
- View tasks by date
- View tasks by year
- View tasks by month
- View tasks graphically by year
- View tasks graphically by month
- View tasks graphically by category

### Technologies
- C#
- Entity Framework
- SQL Server
- Windows Forms
- Chart.js

### Installation
- Clone the repository
- ceate a database in MSSQL Server
- create a table in the database with the following columns
```sql
CREATE TABLE CATEGORY (
    categoryID INT PRIMARY KEY AUTO_INCREMENT,
    categoryName VARCHAR(50) NOT NULL,
    isDeleted BIT NOT NULL,
    deletedDate DATETIME
);

CREATE TABLE TASK (
    taskID INT PRIMARY KEY AUTO_INCREMENT,
    categoryID INT NOT NULL,
    timeSpent INT NOT NULL,
    day INT NOT NULL,
    monthID INT NOT NULL,
    year INT NOT NULL,
    dailyRoutineID INT NOT NULL,
    isDeleted BIT NOT NULL,
    deletedDate DATETIME NULL,
    summary text NULL,
    FOREIGN KEY (categoryID) REFERENCES
    FOREIGN KEY (monthID) REFERENCES
    FOREIGN KEY (dailyRoutineID) REFERENCES
    );

    CREATE TABLE DAILYROUTINE (
    dailyRoutineID INT PRIMARY KEY AUTO_INCREMENT,
    routineDate DATETIME NOT NULL,
    day INT NOT NULL,
    monthID INT NOT NULL,
    year INT NOT NULL,
    summary text NULL,
    isDeleted BIT NOT NULL,
    deletedDate DATETIME NULL
    FOREIGN KEY (monthID) REFERENCES
    );

    CREATE TABLE MONTH (
    monthID INT PRIMARY KEY AUTO_INCREMENT,
    monthName VARCHAR(50) NOT NULL,
    );
```
- Update the connection string in the `App.config` file
- Open the project in Visual Studio
- Run the project

### License
Permission is hereby granted, free of charge, to any Firm and HR Recruiter obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify and to merge.
