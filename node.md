add migration 
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api --output-dir DataAccess\Migrations

update database
dotnet ef database update  --project Infrastructure --startup-project Api

check
dotnet ef dbcontext info --project Infrastructure --startup-project Api
