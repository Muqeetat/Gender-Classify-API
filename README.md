# Gender Classify API

A high-performance .NET Minimal API that integrates with the Genderize.io service to predict the gender of a name with custom confidence logic and data processing.

## 🚀 Features
- **Data Transformation**: Renames external API fields (e.g., `count` to `sample_size`).
- **Confidence Logic**: Custom boolean `is_confident` based on probability ($\ge 0.7$) and sample size ($\ge 100$).
- **Input Validation**: Handles missing names (400) and non-string inputs (422).
- **CORS Enabled**: Configured with `Access-Control-Allow-Origin: *` for external grading script compatibility.
- **Performance**: Optimized to ensure internal processing stays under 500ms.

---

## 🛠 Tech Stack
* **.NET 9** (Minimal APIs)
* **C# 13**
* **System.Text.Json** (Snake_case serialization)
* **IHttpClientFactory** (Efficient external API consumption)

---

## 🏃 Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or 9.0)

### Installation
1. Clone the repository:
   ```bash
   git clone <your-repo-url>
   cd GenderClassifyApi
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

## 📖 API Documentation

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

## 🧪 Testing

### Postman Testing
To verify the requirements, use the following test script in the **Tests** tab of Postman:

```javascript
pm.test("Status code is 200", () => pm.response.to.have.status(200));

pm.test("CORS header is present", () => {
    pm.response.to.have.header("Access-Control-Allow-Origin", "*");
});

pm.test("Confidence logic is correct", () => {
    const data = pm.response.json().data;
    const expected = data.probability >= 0.7 && data.sample_size >= 100;
    pm.expect(data.is_confident).to.eql(expected);
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

## 🔗 Deployment
**Public API URL:** ``  
**GitHub Repository:** `https://github.com/Muqeetat/Gender-Classify-API.git`

---

### Implementation Details: The Middleware Pipeline


The application follows a strict middleware order to ensure CORS headers are attached to every response, including validation errors. The logic is decoupled into a Service layer to maintain clean code standards.