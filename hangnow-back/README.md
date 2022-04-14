
### Setup db bash instructions

```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update
```

To create a migration for the database:
```bash
dotnet ef migrations add SomeTableChange
```