# GridView - Custom Pagination

We can use GridView + SqlDataSource to get all the data easily. But, when come to a huge set of data it will have the performance issue. 

Example, 100k rows of data in the database table and it will pull 100k rows of data and bind into the GridView. This action will be performed every time when user click on the page selection. 

This is why the custom pagination come in and help resolve the performance issue. 

Example, 10k rows of data in the database table and it will only pull 20 rows of data (we defined how many we want) and bind into the GridView.

#### *Note: this is just a sample project as a reference*

___

#### Tools:
- Microsoft SQL Server Management Studio
- Visual Studio 2019

#### Steps:
1. Create database - MOCK_DB
2. Import dummy data - MOCK_DATA.sql
3. Open the project "CustomPagination" with Visual Studio 2019 and run it

#### Youtube: 
https://youtu.be/wTdDqBcONco

#### Screenshot:

![GridView - Custom Pagination](https://raw.githubusercontent.com/joannakoay616/CustomPagination/main/screenshot.png)
