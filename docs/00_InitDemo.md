
# Demo Data Initialization for RentAutoApp

## 0. InitDemo

Demo data and images are prepared in the `Install/` folder.

1. Apply EF Core migrations:
   ```
   dotnet ef database update
   ```

2. Start the project:
   ```
   dotnet run --project src/RentAutoApp.Web
   ```

3. This will seed:
   - 3 demo users: Admin, Staff, and Client
   - Sample locations, vehicles, reservations, contracts, etc.

4. Extract the provided `images.zip` from `Install/` into:
   ```
   src/RentAutoApp.Web/wwwroot/images/
   ```

   This ensures vehicles display with demo images correctly.

5. Demo logins:

| Role           | Email                    | Password      |
|----------------|--------------------------|---------------|
| Administrator  | admin@rentauto.local     | Admin123!     |
| Staff          | staff@rentauto.local     | Staff123!     |
| Client         | client@rentauto.local    | Client123!    |

---

## 1. Database Preparation

1. Make sure your database is up to date:
```
dotnet ef database update
```

2. Start the project:
```
dotnet run --project src/RentAutoApp.Web
```

3. This will trigger `DbSeeder`, which will insert:
   - 3 users: admin, staff, and client
   - 2-3 locations
   - 5 demo vehicles
   - All necessary demo data (contracts, reservations, etc.)

---

## 2. Image Preparation

1. In the `Install/` folder you will find the archive `images.zip`, which contains all required vehicle images in the following structure:

```
images/
├── 1/
│   ├── image1.jpg
│   ├── image2.jpg
│   └── image5.jpg
├── 2/
...
├── 5/
```

2. Extract the contents of `images.zip` directly into:

```
src/RentAutoApp.Web/wwwroot/images/
```

This will result in the following structure:

```
wwwroot/images/1/image1.jpg
wwwroot/images/2/image3.jpg
...
```

**No need to rename or download anything — everything is prepared!**

---

## 3. Demo Accounts

| Role           | Email                    | Password      |
|----------------|--------------------------|---------------|
| Administrator  | admin@rentauto.local     | Admin123!     |
| Staff          | staff@rentauto.local     | Staff123!     |
| Client         | client@rentauto.local    | Client123!    |

---

## 4. Running the Application

- Open `https://localhost:5001`
- Log in using one of the demo accounts above
- Browse the vehicle list and view images

---

## Notes

After the demonstration, you can remove all demo data.
