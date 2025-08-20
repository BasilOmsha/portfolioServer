# Portfolio Contact API

A secure ASP.NET Core Web API for handling contact form submissions with EmailJS integration and reCAPTCHA validation.

## 🛡️ Security Features

- **Server-side reCAPTCHA validation** - Prevents bot submissions
- **EmailJS integration** - Secure email sending without exposing credentials
- **User Secrets** - Sensitive data never committed to source control
- **Clean Architecture** - Separation of concerns and maintainable code
- **CORS configured** - Ready for frontend integration

## 🚀 Quick Start

### 1. Configure User Secrets

```bash
# Navigate to the API project
cd src/Portfolio.Api

# Set EmailJS configuration
dotnet user-secrets set "EmailJsSettings:ServiceId" "your_emailjs_service_id"
dotnet user-secrets set "EmailJsSettings:TemplateId" "your_emailjs_template_id"
dotnet user-secrets set "EmailJsSettings:PublicKey" "your_emailjs_public_key"
dotnet user-secrets set "EmailJsSettings:PrivateKey" "your_emailjs_private_key"

# Set reCAPTCHA configuration
dotnet user-secrets set "RecaptchaSettings:SiteKey" "your_recaptcha_site_key"
dotnet user-secrets set "RecaptchaSettings:SecretKey" "your_recaptcha_secret_key"
```

### 2. Build and Run

```bash
# Build the solution
dotnet build

# Run the API
dotnet run --project src/Portfolio.Api
```

The API will be available at `https://localhost:7000` (or the port shown in console).

### 3. Test the Endpoint

```bash
POST https://localhost:7000/api/contact
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "message": "Hello from the API!",
  "recaptchaToken": "valid_recaptcha_token"
}
```

## 📁 Project Structure

```
src/
├── Portfolio.Api/           # Web API layer (controllers, middleware)
├── Portfolio.Application/   # Business logic (services, DTOs, interfaces)
├── Portfolio.Domain/        # Domain models and common classes
└── Portfolio.Infrastructure # Configuration and settings
```

## 🔧 Configuration

### Development (User Secrets - Recommended)
Sensitive configuration is stored in user secrets (never committed):

```bash
# View current secrets
dotnet user-secrets list --project src/Portfolio.Api

# Clear all secrets if needed
dotnet user-secrets clear --project src/Portfolio.Api
```

### Production
For production deployment, use:
- Azure Key Vault
- Environment variables  
- Docker secrets
- Kubernetes secrets

## 📡 API Endpoints

### POST /api/contact
Creates a new contact form submission.

**Request Body:**
```json
{
  "name": "string (required)",
  "email": "string (required, valid email)",
  "message": "string (required)",
  "recaptchaToken": "string (required)"
}
```

**Success Response (201 Created):**
```json
{
  "message": "Message sent successfully!"
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Validation failed",
  "errors": {
    "Email": ["Invalid email format."],
    "RecaptchaToken": ["reCAPTCHA token is required."]
  }
}
```

## 🔐 Security

- ✅ **reCAPTCHA validation** - Server-side verification prevents bots
- ✅ **User secrets** - No sensitive data in source control
- ✅ **CORS policy** - Restricted to allowed origins
- ✅ **Model validation** - Input sanitization and validation
- ✅ **Exception handling** - Global error handling middleware

## 🌐 Frontend Integration

See `FRONTEND_INTEGRATION.md` for detailed instructions on updating your frontend to use this API instead of direct EmailJS calls.

## 📚 Technologies

- **ASP.NET Core 8** - Web API framework
- **Clean Architecture** - Domain-driven design
- **EmailJS** - Email service integration
- **reCAPTCHA v2** - Bot protection
- **User Secrets** - Secure configuration management