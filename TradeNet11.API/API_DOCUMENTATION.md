# TradeNet11 API Documentation

## Overview
The TradeNet11 API is a RESTful API for managing trade compliance operations including audits, compliance cases, and compliance officers.

## Architecture

### Layers
- **Controllers**: HTTP request handling and routing (API Controllers)
- **Services**: Business logic and orchestration (IAuditService, IComplianceCaseService, etc.)
- **Repositories**: Data access layer (IAuditRepository, IComplianceCaseRepository, etc.)
- **Models**: Domain entities (Audit, ComplianceCase, ComplianceOfficer, etc.)
- **DTOs**: Data transfer objects for API requests/responses
- **Data Context**: Entity Framework Core DbContext for database access

## API Endpoints

### Base URL
```
https://localhost:7xxx/api
```

### Audits Endpoints

#### GET /api/audits
Retrieve all audits.

**Response:**
```json
{
  "success": true,
  "message": "Audits retrieved successfully",
  "data": [
    {
      "id": 1,
      "auditTitle": "Q4 2024 Audit",
      "businessName": "ABC Trading Co.",
      "status": "Scheduled",
      "scheduledDate": "2024-12-15T00:00:00Z",
      "completedDate": null,
      "findings": null,
      "checklistJson": null,
      "assignedOfficerId": 1,
      "assignedOfficerName": "John Doe",
      "complianceCaseId": null
    }
  ],
  "statusCode": 200
}
```

#### GET /api/audits/{id}
Retrieve audit details by ID.

**Parameters:**
- `id` (path parameter): Audit ID

**Response:**
```json
{
  "success": true,
  "message": "Audit retrieved successfully",
  "data": {
    "id": 1,
    "auditTitle": "Q4 2024 Audit",
    "businessName": "ABC Trading Co.",
    "status": "Scheduled",
    "scheduledDate": "2024-12-15T00:00:00Z",
    "completedDate": null,
    "findings": null,
    "checklistJson": null,
    "assignedOfficerId": 1,
    "assignedOfficerName": "John Doe",
    "complianceCaseId": null
  },
  "statusCode": 200
}
```

#### POST /api/audits
Create a new audit.

**Request Body:**
```json
{
  "auditTitle": "Q4 2024 Audit",
  "businessName": "ABC Trading Co.",
  "scheduledDate": "2024-12-15T00:00:00Z",
  "assignedOfficerId": 1,
  "complianceCaseId": null,
  "checklistJson": null
}
```

**Response:** 201 Created

#### PUT /api/audits/{id}
Update an existing audit.

**Parameters:**
- `id` (path parameter): Audit ID

**Request Body:**
```json
{
  "auditTitle": "Q4 2024 Audit - Updated",
  "businessName": "ABC Trading Co.",
  "scheduledDate": "2024-12-20T00:00:00Z",
  "assignedOfficerId": 1,
  "checklistJson": null
}
```

**Response:** 200 OK

#### POST /api/audits/{id}/start
Start an audit (change status from Scheduled to InProgress).

**Parameters:**
- `id` (path parameter): Audit ID

**Response:** 200 OK

#### POST /api/audits/{id}/complete
Complete an audit with findings.

**Parameters:**
- `id` (path parameter): Audit ID

**Request Body:**
```json
{
  "findings": "No major violations found. Minor documentation issues noted."
}
```

**Response:** 200 OK

#### DELETE /api/audits/{id}
Delete an audit.

**Parameters:**
- `id` (path parameter): Audit ID

**Response:** 200 OK

---

### Compliance Cases Endpoints

#### GET /api/compliancecases
Retrieve all compliance cases.

**Response:** 200 OK with list of compliance cases

#### GET /api/compliancecases/{id}
Retrieve compliance case details by ID.

**Parameters:**
- `id` (path parameter): Compliance Case ID

#### POST /api/compliancecases
Create a new compliance case.

**Request Body:**
```json
{
  "caseName": "Case-001",
  "businessName": "XYZ Import Co.",
  "description": "Customs violation case",
  "assignedOfficerId": 1
}
```

**Response:** 201 Created

#### PUT /api/compliancecases/{id}
Update an existing compliance case.

**Parameters:**
- `id` (path parameter): Compliance Case ID

#### POST /api/compliancecases/{id}/close
Close a compliance case.

**Parameters:**
- `id` (path parameter): Compliance Case ID

**Response:** 200 OK

#### DELETE /api/compliancecases/{id}
Delete a compliance case.

**Parameters:**
- `id` (path parameter): Compliance Case ID

---

### Compliance Officers Endpoints

#### GET /api/complianceofficers
Retrieve all compliance officers.

**Response:** 200 OK with list of officers

#### GET /api/complianceofficers/{id}
Retrieve compliance officer details by ID.

**Parameters:**
- `id` (path parameter): Officer ID

#### POST /api/complianceofficers
Create a new compliance officer.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john.doe@tradenet.com",
  "phone": "555-0123",
  "department": "Compliance"
}
```

**Response:** 201 Created

#### PUT /api/complianceofficers/{id}
Update an existing compliance officer.

**Parameters:**
- `id` (path parameter): Officer ID

#### DELETE /api/complianceofficers/{id}
Delete a compliance officer.

**Parameters:**
- `id` (path parameter): Officer ID

---

## Swagger/OpenAPI Documentation

The API includes interactive Swagger documentation. Once the API is running:

1. Navigate to `https://localhost:7xxx/` (or the root of the API)
2. You'll be automatically redirected to the Swagger UI
3. All endpoints are documented with descriptions, parameters, and example responses

### Features:
- Try-it-out functionality to test endpoints directly
- Request/response examples
- Parameter validation
- HTTP status code documentation

---

## Testing with Postman

### Import the API into Postman

#### Option 1: Import from Swagger URL
1. Open Postman
2. Click "Import" (or use Ctrl+K)
3. Go to the "Link" tab
4. Paste: `https://localhost:7xxx/swagger/v1/swagger.json`
5. Click "Import"

#### Option 2: Manual Collection Creation
1. Create a new Postman Collection named "TradeNet11 API"
2. Add requests manually using the examples below

### Sample Postman Requests

#### 1. Create an Audit
```
POST https://localhost:7xxx/api/audits
Content-Type: application/json

{
  "auditTitle": "Q4 2024 Compliance Audit",
  "businessName": "ABC Trading Co.",
  "scheduledDate": "2024-12-15T00:00:00Z",
  "assignedOfficerId": 1,
  "complianceCaseId": null,
  "checklistJson": null
}
```

#### 2. Get All Audits
```
GET https://localhost:7xxx/api/audits
```

#### 3. Get Specific Audit
```
GET https://localhost:7xxx/api/audits/1
```

#### 4. Start an Audit
```
POST https://localhost:7xxx/api/audits/1/start
```

#### 5. Complete an Audit
```
POST https://localhost:7xxx/api/audits/1/complete
Content-Type: application/json

{
  "findings": "Audit completed successfully. No violations found."
}
```

#### 6. Update an Audit
```
PUT https://localhost:7xxx/api/audits/1
Content-Type: application/json

{
  "auditTitle": "Q4 2024 Compliance Audit - Updated",
  "businessName": "ABC Trading Co.",
  "scheduledDate": "2024-12-20T00:00:00Z",
  "assignedOfficerId": 1,
  "checklistJson": null
}
```

#### 7. Delete an Audit
```
DELETE https://localhost:7xxx/api/audits/1
```

#### 8. Create a Compliance Case
```
POST https://localhost:7xxx/api/compliancecases
Content-Type: application/json

{
  "caseName": "CASE-2024-001",
  "businessName": "XYZ Import Co.",
  "description": "Suspected tariff violation",
  "assignedOfficerId": 1
}
```

#### 9. Get All Compliance Cases
```
GET https://localhost:7xxx/api/compliancecases
```

#### 10. Close a Compliance Case
```
POST https://localhost:7xxx/api/compliancecases/1/close
```

#### 11. Create a Compliance Officer
```
POST https://localhost:7xxx/api/complianceofficers
Content-Type: application/json

{
  "name": "Jane Smith",
  "email": "jane.smith@tradenet.com",
  "phone": "555-0456",
  "department": "Compliance"
}
```

#### 12. Get All Compliance Officers
```
GET https://localhost:7xxx/api/complianceofficers
```

---

## Running the API

### Prerequisites
- .NET 10 SDK installed
- SQL Server or LocalDB configured
- Connection string configured in `appsettings.json`

### Steps

1. **Update appsettings.json** (in TradeNet11.API project)
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TradeNet11;Trusted_Connection=true;"
     }
   }
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run database migrations** (from TradeNet11 project)
   ```bash
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run --project TradeNet11.API
   ```

5. **Access the API**
   - Swagger UI: `https://localhost:7xxx/`
   - API Base: `https://localhost:7xxx/api/`

---

## Response Format

All API responses follow a consistent format:

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* actual data */ },
  "statusCode": 200
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "statusCode": 400
}
```

---

## HTTP Status Codes

- **200 OK**: Request successful
- **201 Created**: Resource successfully created
- **400 Bad Request**: Invalid request data
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

---

## Security Considerations

- All endpoints use HTTPS in production
- CORS is configured to allow all origins (for development)
- Implement authentication/authorization as needed (JWT, OAuth2, etc.)
- Validate all input data
- Use HTTPS only in production

---

## Project Structure

```
TradeNet11.API/
├── Controllers/
│   ├── AuditsController.cs
│   ├── ComplianceCasesController.cs
│   └── ComplianceOfficersController.cs
├── DTOs/
│   ├── AuditDto.cs
│   ├── ComplianceCaseDto.cs
│   ├── ComplianceOfficerDto.cs
│   └── ApiResponse.cs
├── Program.cs
└── TradeNet11.API.csproj

(Services, Repositories, and Models are shared from TradeNet11 project)
```

---

## Troubleshooting

### Common Issues

**Issue: 404 Not Found on endpoints**
- Ensure API is running
- Verify correct port number
- Check endpoint routes match documentation

**Issue: 500 Internal Server Error**
- Check database connection string
- Verify database migrations have run
- Check API logs for detailed error messages

**Issue: CORS errors**
- CORS is enabled for development (all origins allowed)
- Configure as needed for production

---

## Support

For issues or questions:
1. Check the Swagger documentation at the API root
2. Review error messages in the response
3. Check application logs
4. Review the source code in Controllers and Services

