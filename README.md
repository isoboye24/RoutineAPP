### RoutineAPP – Daily Productivity Analytics System

## Overview
A production-used, 3-tier desktop analytics system for structured time tracking and statistical productivity reporting.

The application was initially designed for structured personal productivity tracking
but was architected using a modular and layered design, making it suitable for
general-purpose time management and analytical reporting.

The system has been actively used in production for over two years, demonstrating
long-term stability, maintainability, and real-world usability.

## Architecture
The application follows a structured 3-Tier Layered Architecture, ensuring 
separation of concerns and maintainable code organization.

#### Presentation Layer
- Windows Forms (UI)
- Responsible for user interaction and data presentation
- Located in AllForms
- Fully decoupled from database logic

#### Business Logic Layer (BLL)
- Encapsulates application rules
- Handles validation and aggregation logic
- Coordinates communication between UI and Data Layer
- Implements generic IBLL<T> interface for structured abstraction
- Centralizes calculation and reporting logic

#### Data Access Layer (DAL)
- DAO (Data Access Object) pattern implementation
- DTO (Data Transfer Object) pattern for structured data exchange
- Entity Framework (Database-First, EDMX model)
- Soft delete strategy (isDeleted, deletedDate) for safe record management
- Encapsulated and isolated database operations


## Analytics & Reporting Engine
The system includes a fully integrated productivity analytics module with structured
statistical reporting capabilities.
#### Core Features:
- Daily activity logging with persistent storage
- Category-based time tracking and distribution
- Monthly aggregated reporting (GROUP BY month logic)
- Yearly productivity summaries per category
- Total hours per category per year
- Average hours per month calculations
- Dynamic bar chart visualization per selected category
- Cross-category comparison dashboard (monthly & annual)
- Time-series sorting and historical search functionality
- Centralized dashboard summary screen for productivity overview


#### Statistical Processing

Data aggregation and statistical calculations are performed using structured LINQ queries and database-level filtering to ensure:


- Accurate time summation
- Efficient grouped queries
- Optimized ordering by year → month → day
- Clean separation between calculation logic (BLL) and persistence logic (DAL)

This design ensures scalability, maintainability, and clear responsibility 
boundaries between layers.

````
+--------------------------------------------------+
|                  Presentation Layer              |
|--------------------------------------------------|
| Windows Forms (AllForms)                         |
| - FormCategory                                   |
| - FormCategoryList                               |
| - FormDailyRoutine                               |
| - FormDailyRoutineList                           |
| - FormDeletedData                                |
| - FormGraphs                                     |
| - FormMonthlyReports                             |
| - FormMonthlyRoutineReportsList                  |
| - FormSummaryList                                |
| - FormTaskList                                   |
| - FormTaskWithSummary                            |
| - FormTotalReportsList                           |
+-------------------------▲------------------------+
                          |
                          |
+-------------------------|------------------------+
|               Business Logic Layer (BLL)         |
|--------------------------------------------------|
| - CategoryBLL                                    |
| - DailyTaskBLL                                   |
| - DashboardBLL                                   |
| - GraphBLL                                       |
| - IBLL                                           |
| - MonthBLL                                       |
| - ReportsBLL                                     |
| - TaskBLL                                        |
| - YearBLL                                        |
|                                                  |
| Handles:                                         |
| - Validation                                     |
| - Aggregation Logic                              |
| - Statistical Calculations                       |
+-------------------------▲------------------------+
                          |
                          |
+-------------------------|------------------------+
|               Data Access Layer (DAL)            |
|--------------------------------------------------|
| DAO                                              |
| - APPContext                                     |
| - CategoryDAO                                    |
| - DailyTaskDAO                                   |
| - GraphDAO                                       |
| - IDAO                                           |
| - MonthDAO                                       |
| - ReportsDAO                                     |
| - TaskDAO                                        |
| - YearDAO                                        |
|                                                  |
| DTO                                              |
| - AllYearsDetailDTO                              |
| - YearDTO                                        |
| - CategoryDetailDTO                              |
| - CategoryDTO                                    |
| - MonthDetailDTO                                 |
| - MonthDTO                                       |
| - DailyTaskDetailDTO                             |
| - DailyTaskDTO                                   |
| - TaskDetailDTO                                  |
| - TaskDTO                                        |
| - MonthlyRoutinesDetailDTO                       |
| - MonthlyRoutineDTO                              |
| - ReportsDetailDTO                               |
| - ReportDTO                                      |
| - GraphDetailDTO                                 |
| - GraphDTO                                       |
|                                                  |
| Entity Framework (EDMX - Database First)         |
+-------------------------▲------------------------+
                          |
                          |
+-------------------------|------------------------+
|                    Microsoft SQL Server          |
|--------------------------------------------------|
| Tables:                                          |
| - DAILY_ROUTINE                                  |
| - MONTHs                                         |
| - TASK                                           |
| - CATEGORY                                       |
+--------------------------------------------------+
````

## Data Flow
````

1. User interacts with Windows Forms UI.
2. UI sends request to BLL.
3. BLL performs validation and statistical calculations.
4. BLL calls DAO layer.
5. DAO interacts with Entity Framework.
6. Entity Framework executes queries against MSSQL.
7. Results are mapped into DTOs and returned upward.
````

## 🗄 Database Schema
````

    CATEGORY {
        int categoryID PK
        string categoryName
        bool isDeleted
        datetime deletedDate
    }

    MONTH {
        int monthID PK
        string monthName
    }

    DAILY_ROUTINE {
        int dailyRoutineID PK
        datetime routineDate
        string summary
        int day
        int monthID FK
        int year
        bool isDeleted
        datetime deletedDate
    }

    TASK {
        int taskID PK
        int categoryID FK
        int dailyRoutineID FK
        int monthID FK
        int year
        int day
        decimal timeSpent
        string summary
        bool isDeleted
        datetime deletedDate
    }

    MONTH ||--o{ DAILY_ROUTINE : contains
    DAILY_ROUTINE ||--o{ TASK : includes
    CATEGORY ||--o{ TASK : classifies
    MONTH ||--o{ TASK : groups
````

````
 Database Design Overview

The database follows a normalized relational structure:

- DAILY_ROUTINE represents a specific calendar day entry.
- TASK represents categorized time entries associated with a routine.
- CATEGORY provides classification for activity tracking.
- MONTH acts as a lookup reference table.

Soft delete strategy is implemented across core tables using:
- isDeleted
- deletedDate

This ensures historical consistency and safe record management.
````


## 🏠 Dashboard Overview
Displays total hours per category, yearly summaries, and average monthly statistics.


![Dashboard](images/dashboard.png)


## 📅 Task Entry & View
Interface for adding each task with optional summary and daily tasks view.

<p align="center">
  <img src="images/daily_tasks.png" width="32%" height="150"/>
  <img src="images/add_task.png" width="32%" height="150"/>
  <img src="images/task_view.png" width="32%" height="150" />
</p>


## Reports
Aggregated monthly and annual time distribution per category, and 
Bar chart visualization of annual category performance


<p align="center">
  <img src="images/month_report.png" width="32%" height="150"/>
  <img src="images/yearly_barchart.png" width="32%" height="150"/>
  <img src="images/annual.png" width="32%" height="150" />
</p>


## Technical Highlights

- Clean 3-tier layered architecture
- Separation of UI, business logic, and data access concerns
- Soft delete implementation across core entities
- LINQ-based aggregation and statistical computation
- DTO-based data transport for decoupled communication
- Production-validated stability over extended use

## Technologies Used
- C#
- .NET Framework
- Windows Forms
- Microsoft SQL Server
- Entity Framework (Database-First / EDMX)
- LINQ
- System.Windows.Forms.DataVisualization (Charts)
- Layered Architecture (3-Tier)
- DAO & DTO Patterns

## Production Stability

The system has been actively used in real-world daily operations for two years, demonstrating:

- Architectural robustness
- Data consistency
- Reporting accuracy
- Long-term maintainability


## 🚀 Installation

### Prerequisites

- Windows OS
- Visual Studio (2022 or later recommended)
- .NET Framework 4.7.2 or later
- Microsoft SQL Server SQLEXPRESS
- SQL Server Management Studio 

---

### 1️. Clone the Repository

```bash
git clone https://github.com/isoboye24/RoutineAPP.git

````

### 2. Open the Solution

Open RoutineAPP.sln in Visual Studio.


### 3. Database Configuration

RoutineAPP is configured to use a local SQL Server Express instance with 
Windows Integrated Security.

The connection string is preconfigured in `App.config` and does not require 
manual adjustment for standard local environments.

Requirements:

- Microsoft SQL Server Express installed
- SQL Server (SQLEXPRESS) service running
- Windows Authentication enabled

On first execution, the application automatically verifies the existence of 
the target database and creates it if it does not already exist.


### 4. Build and Run

- Build the solution (Ctrl + Shift + B)
- Run the application (F5)



### License
This project is provided for portfolio and demonstration purposes only.
Unauthorized commercial use is not permitted.
