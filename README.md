# Gender Classify API

A high-performance .NET Minimal API that integrates with the Genderize.io service to predict the gender of a name with custom confidence logic and data processing.

## Features
- **Data Transformation**: Renames external API fields (e.g., `count` to `sample_size`).
- **Confidence Logic**: Custom boolean `is_confident` based on probability ($\ge 0.7$) and sample size ($\ge 100$).
- **Input Validation**: Handles missing names (400) and non-string inputs (422).
- **CORS Enabled**: Configured with `Access-Control-Allow-Origin: *` for external grading script compatibility.
- **Performance**: Optimized to ensure internal processing stays under 500ms.

---

## Tech Stack
* **.NET 9** (Minimal APIs)
* **C# 13**
* **System.Text.Json** (Snake_case serialization)
* **IHttpClientFactory** (Efficient external API consumption)

---

## Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download) (Version 9.0 or 10.0)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Muqeetat/Gender-Classify-API.git
   cd GenderClassifyAPI
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5xxx/api/classify`.

---

## API Documentation

### GET `/api/classify`
Fetches gender prediction for a given name.

**Query Parameters:**
* `name` (string, required): The name to classify.

**Success Response (200 OK):**
```json
{
  "status": "success",
  "data": {
    "name": "Alex",
    "gender": "male",
    "probability": 0.9,
    "sample_size": 1500,
    "is_confident": true,
    "processed_at": "2026-04-13T12:00:00Z"
  }
}
```

---

## Testing

### Postman Testing
To verify the requirements, use the following test script in the **Tests** tab of Postman:

```javascript
// 1. Verify the HTTP Status Code
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// 2. Verify CORS Header
pm.test("CORS header Access-Control-Allow-Origin is *", function () {
    pm.response.to.have.header("Access-Control-Allow-Origin", "*");
});

// 3. Verify the JSON structure matches the 'Expected response format'
pm.test("Response structure is valid", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('status');
    pm.expect(jsonData).to.have.property('data');
    
    const data = jsonData.data;
    pm.expect(data).to.have.property('name');
    pm.expect(data).to.have.property('gender');
    pm.expect(data).to.have.property('probability');
    pm.expect(data).to.have.property('sample_size');
    pm.expect(data).to.have.property('is_confident');
    pm.expect(data).to.have.property('processed_at');
});

// 4. Verify the Confidence Logic
pm.test("Confidence logic check", function () {
    const data = pm.response.json().data;
    const manuallyCalculated = (data.probability >= 0.7 && data.sample_size >= 100);
    pm.expect(data.is_confident).to.eql(manuallyCalculated);
});

// 5. Verify Date format (ISO 8601)
pm.test("Processed_at is ISO 8601 / UTC format", function () {
    const data = pm.response.json().data;
    // Check if it ends with Z (UTC) and follows the date pattern
    pm.expect(data.processed_at).to.match(/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$/);
});

// 6. Performance check (Excluding external API latency)
// Since we can't easily isolate latency in Postman without custom headers, 
// we check if the overall response is reasonable.
pm.test("Total response time is healthy", function () {
    pm.expect(pm.response.responseTime).to.be.below(2000); 
});
```

### Manual Test Cases
| Scenario | Input | Expected Status |
| :--- | :--- | :--- |
| Valid Name | `?name=Peter` | `200 OK` |
| Empty Name | `?name=` | `400 Bad Request` |
| Numeric Input | `?name=123` | `422 Unprocessable Entity` |
| Unknown Name | `?name=xyz123` | `200 OK` (Error JSON) |

---

## Deployment
**Public API URL:** `gender-classify-api-production-91bb.up.railway.app`  
**GitHub Repository:** `https://github.com/Muqeetat/Gender-Classify-API.git`

---

### The Middleware Pipeline


The application follows a strict middleware order to ensure CORS headers are attached to every response, including validation errors. The logic is decoupled into a Service layer to maintain clean code standards.