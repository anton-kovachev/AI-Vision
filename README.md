# 🔍 AI.Vision - Intelligent Receipt Scanner

> AI-powered receipt scanning and data extraction using .NET Aspire and Ollama Vision Models

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Aspire](https://img.shields.io/badge/Aspire-Enabled-512BD4)](https://learn.microsoft.com/dotnet/aspire/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

**AI.Vision** is a modern, cloud-native receipt scanning application that leverages the power of AI vision models to extract structured data from receipt images. Built with .NET Aspire for distributed application orchestration and Ollama for local AI inference, this project demonstrates how to create production-ready AI applications without relying on cloud-based AI services.

---

## ✨ Features

- 🤖 **AI-Powered OCR**: Uses LLaMA 3.2 Vision model for accurate receipt data extraction
- 📊 **Structured Data Output**: Returns JSON with line items, taxes, totals, and merchant information
- 🚀 **Cloud-Native Architecture**: Built on .NET Aspire for seamless orchestration and scaling
- 🐳 **Containerized Deployment**: Runs Ollama in Docker for consistent environments
- 📖 **Interactive API Documentation**: Swagger UI for easy testing and exploration
- 🔍 **Smart Validation**: Verifies receipt authenticity before processing
- 💰 **Financial Insights**: Provides summaries with spending analysis
- 🏥 **Health Monitoring**: Built-in health checks and readiness probes

---

## 🛠️ Tech Stack

- **Framework**: .NET 10.0
- **Architecture**: .NET Aspire (Cloud-native distributed application)
- **AI Model**: Ollama (LLaMA 3.2 Vision)
- **API**: ASP.NET Core Minimal APIs
- **AI Integration**: Microsoft.Extensions.AI + OllamaSharp
- **Documentation**: Swagger/OpenAPI
- **Container Runtime**: Docker

---

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (running)
- [Visual Studio 2022 (v17.14+)](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
- [.NET Aspire Workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling)
  ```powershell
  dotnet workload install aspire
  ```

**System Requirements**:

- **Memory**: 8GB RAM minimum (16GB recommended for optimal Ollama performance)
- **Storage**: 10GB free space (for Ollama model downloads)
- **OS**: Windows 10/11, macOS, or Linux

---

## 🚀 Getting Started

### 1️⃣ Clone the Repository

```powershell
git clone https://github.com/yourusername/AI.Vision.git
cd AI.Vision
```

### 2️⃣ Start Docker Desktop

Ensure Docker Desktop is running before launching the application.

### 3️⃣ Run the Application

**Option A: Using Visual Studio**

1. Open `AI.Vision.slnx` in Visual Studio 2022
2. Set `AI.Vision` (AppHost) as the startup project
3. Press `F5` or click **Run**

**Option B: Using Command Line**

```powershell
cd AI.Vision
dotnet run
```

### 4️⃣ Access the Application

Once started, Aspire will:

1. Launch the **Aspire Dashboard** (usually at `http://localhost:15888`)
2. Pull the Ollama Docker image (if not already present)
3. Download the LLaMA 3.2 Vision model (~7GB - first run only)
4. Start the API server

**Important**: Initial startup may take 5-15 minutes depending on your internet speed (model download).

### 5️⃣ Test the API

Navigate to the API endpoint shown in the Aspire Dashboard (typically `http://localhost:XXXX`):

- **Swagger UI**: `http://localhost:XXXX/` - Interactive API documentation
- **API Endpoint**: `POST http://localhost:XXXX/api/scan-receipt`

---

## 📸 Usage

### Upload a Receipt Image

**Using Swagger UI:**

1. Navigate to the Swagger UI homepage
2. Click on **POST /api/scan-receipt**
3. Click **Try it out**
4. Upload a receipt image (PNG, JPG, JPEG)
5. Click **Execute**

**Using cURL:**

```bash
curl -X POST "http://localhost:XXXX/api/scan-receipt" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@receipt.jpg"
```

**Using PowerShell:**

```powershell
$form = @{
    file = Get-Item -Path "receipt.jpg"
}
Invoke-RestMethod -Uri "http://localhost:XXXX/api/scan-receipt" -Method Post -Form $form
```

### Sample Response

```json
{
  "fileName": "receipt.jpg",
  "merchant": "Target Store #1234",
  "lineItems": [
    {
      "name": "Organic Bananas",
      "price": 3.99,
      "code": "12345"
    },
    {
      "name": "Milk 2%",
      "price": 4.29,
      "code": "67890"
    }
  ],
  "taxes": [
    {
      "name": "Sales Tax",
      "rate": 8.25,
      "amount": 0.68
    }
  ],
  "subTotalAmount": 8.28,
  "totalAmount": 8.96,
  "summary": "Grocery receipt with reasonable pricing. Items purchased at standard market rates."
}
```

---

## 🎯 API Endpoints

| Method | Endpoint            | Description                          |
| ------ | ------------------- | ------------------------------------ |
| `POST` | `/api/scan-receipt` | Upload receipt image for AI analysis |
| `GET`  | `/`                 | Swagger UI documentation             |
| `GET`  | `/health`           | Health check endpoint                |
| `GET`  | `/alive`            | Liveness probe                       |
| `GET`  | `/ready`            | Readiness probe                      |

---

## 📁 Project Structure

```
AI.Vision/
├── AI.Vision/                      # Aspire AppHost Project
│   ├── AppHost.cs                  # Orchestration configuration
│   └── appsettings.json           # Aspire settings
├── AI.Vision.Server/              # Web API Project
│   ├── Program.cs                 # API endpoints and configuration
│   ├── Utilities/
│   │   ├── JsonExtractor.cs       # JSON parsing utilities
│   │   └── OllamaConnectionStringParser.cs
│   └── appsettings.json          # API settings
├── test-receipts/                 # Sample receipt images
├── AI.Vision.slnx                # Solution file
└── README.md                      # This file
```

---

## 🧩 How It Works

1. **Image Upload**: User uploads a receipt image via the API
2. **AI Processing**: Image is sent to Ollama's LLaMA 3.2 Vision model
3. **Data Extraction**: AI model analyzes the image and extracts:
   - Merchant information
   - Individual line items (name, price, product code)
   - Tax breakdowns
   - Subtotal and total amounts
4. **Validation**: System verifies calculations and data consistency
5. **Analysis**: AI provides spending insights and price evaluation
6. **Response**: Structured JSON data returned to the client

---

## 🔧 Configuration

### Changing the AI Model

Edit [AppHost.cs](AI.Vision/AppHost.cs) to use a different model:

```csharp
// Default: llama3.2-vision:latest
var visionModel = ollama.AddModel("vision-model", "llama3.2-vision:latest");

// Alternative: Smaller/faster model
var visionModel = ollama.AddModel("vision-model", "llava:7b");
```

### Adjusting Timeout

Edit [Program.cs](AI.Vision.Server/Program.cs):

```csharp
builder.Services.AddHttpClient("OllamaHClient", config =>
{
    config.BaseAddress = new Uri(endpointUrl);
    config.Timeout = TimeSpan.FromMinutes(30); // Adjust as needed
});
```

---

## 🐛 Troubleshooting

### Issue: Model takes too long to download

**Solution**: Ensure stable internet connection. Monitor progress in Aspire Dashboard under the Ollama container logs.

### Issue: Timeout errors during inference

**Solution**: Increase timeout in `Program.cs` or use a smaller/faster model like `llava:7b`.

### Issue: Docker not running

**Solution**: Start Docker Desktop before running the application.

### Issue: Port conflicts

**Solution**: Aspire automatically assigns ports. Check the Aspire Dashboard for actual service URLs.

### Issue: Out of memory errors

**Solution**: Increase Docker memory allocation in Docker Desktop settings (recommended: 8GB+).

### Issue: Model returns invalid JSON

**Solution**: The `JsonExtractor` utility handles malformed responses. Check logs for parsing errors.

---

## 🧪 Testing

Sample receipt images are provided in the `test-receipts/` directory for testing purposes.

```powershell
# Test with sample receipt
Invoke-RestMethod -Uri "http://localhost:XXXX/api/scan-receipt" `
  -Method Post `
  -Form @{ file = Get-Item "test-receipts/sample-receipt.jpg" }
```

---

## 🔮 Future Enhancements

- [ ] Multi-language receipt support
- [ ] Batch processing for multiple receipts
- [ ] Export to Excel/CSV
- [ ] Integration with accounting software (QuickBooks, Xero)
- [ ] OCR fallback for non-AI extraction
- [ ] Receipt duplicate detection
- [ ] Cloud deployment templates (Azure Container Apps)
- [ ] Real-time receipt scanning mobile app
- [ ] Category classification for expense tracking
- [ ] Historical spending analytics dashboard

---

## 📚 Learn More

- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Ollama Official Site](https://ollama.ai/)
- [LLaMA 3.2 Model Card](https://huggingface.co/meta-llama/Llama-3.2-Vision)
- [OllamaSharp GitHub](https://github.com/awaescher/OllamaSharp)
- [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai)

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 👨‍💻 Author

**Your Name**

- GitHub: [@yourusername](https://github.com/yourusername)
- LinkedIn: [Your Profile](https://linkedin.com/in/yourprofile)

---

## ⭐ Show Your Support

Give a ⭐️ if this project helped you!

---

## 🙏 Acknowledgments

- Meta AI for the LLaMA 3.2 Vision model
- The Ollama team for making AI models accessible
- Microsoft for .NET Aspire and the Microsoft.Extensions.AI library
- The open-source community

---

**Built with ❤️ using .NET Aspire and AI**
