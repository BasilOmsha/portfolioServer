# Frontend Integration Guide

# Frontend Integration Guide

## Updated Frontend Code

Here's how your `Contact.tsx` file should look to work with your ASP.NET Core backend:

### 1. Remove EmailJS Import and Add API Configuration

```typescript
import { useEffect, useRef, useState } from 'react'

// Remove this line - no longer needed:
// import emailjs from '@emailjs/browser'

import { zodResolver } from '@hookform/resolvers/zod'
import ReCAPTCHA from 'react-google-recaptcha'
import { useForm } from 'react-hook-form'
import toast, { Toaster } from 'react-hot-toast'
import { BeatLoader } from 'react-spinners'

import ContactExperience from '../../components/models/contact/ContactExperience.tsx'
import TitleHeader from '../../components/title-header/TitleHeader.tsx'
import { contactFormSchema, type ContactFormData } from '../../schemas/contactForm.ts'

import './Contact.css'

// Add API configuration
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7000/api'

function Contact() {
    const formRef = useRef<HTMLFormForm>(null)
    const [isMouseDown, setIsMouseDown] = useState<boolean>(false)

    const RECAPTCHA_SITE_KEY = import.meta.env.VITE_APP_SITE_KEY
    const recaptchaRef = useRef<ReCAPTCHA>(null)

    // ... your existing useEffect for mouse handling stays the same ...

    const {
        register,
        handleSubmit,
        reset,
        trigger,
        setValue,
        formState: { errors, isSubmitting }
    } = useForm<ContactFormData>({
        resolver: zodResolver(contactFormSchema),
        mode: 'all',
        defaultValues: {
            name: '',
            email: '',
            message: '',
            recaptcha: ''
        }
    })
```

### 2. Replace the onSubmit Function

Replace your existing `onSubmit` function with this new version:

```typescript
    const onSubmit = async (data: ContactFormData): Promise<void> => {
        try {
            // Your existing form validation handles reCAPTCHA
            const isFormValid = await trigger()
            if (!isFormValid) return

            // Prepare data for your ASP.NET Core API
            const contactData = {
                name: data.name,
                email: data.email,
                message: data.message,
                recaptchaToken: data.recaptcha  // This comes from your form validation
            }

            // Call your ASP.NET Core API
            const response = await fetch(`${API_BASE_URL}/contact`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(contactData),
            })

            const result = await response.json()

            if (response.ok) {
                reset()
                if (recaptchaRef.current) {
                    recaptchaRef.current.reset()
                }
                toast.success('I received your message. Get back to you soon!')
            } else {
                // Handle validation errors
                if (result.errors) {
                    const errorMessages = Object.values(result.errors).flat().join(', ')
                    toast.error(errorMessages)
                } else {
                    toast.error(result.error || result.message || 'Failed to send message. Please try again.')
                }
            }
        } catch (error) {
            console.error('API error:', error)
            toast.error('Failed to send message. Please try again.')
        }
    }
```

### 3. Keep Your Existing reCAPTCHA Handlers

Your existing reCAPTCHA handlers remain the same:

```typescript
    const handleRecaptchaChange = (token: string | null) => {
        setValue('recaptcha', token || '', { shouldValidate: true })
    }

    const handleRecaptchaExpired = () => {
        setValue('recaptcha', '', { shouldValidate: true })
        if (recaptchaRef.current) {
            recaptchaRef.current.reset()
        }
    }
```

### 4. Your JSX Stays Exactly the Same

All your JSX (return statement) remains identical - no changes needed!

## Environment Variables Changes

### Remove These (No Longer Needed):
```env
# These are now handled securely by your backend
VITE_APP_EMAILJS_SERVICE_ID=
VITE_APP_EMAILJS_TEMPLATE_ID=
VITE_APP_EMAILJS_PUBLIC_KEY=
```

### Keep This One:
```env
# Still needed for frontend reCAPTCHA widget
VITE_APP_SITE_KEY=your_recaptcha_site_key
```

### Add This One:
```env
# Your backend API URL
VITE_API_BASE_URL=https://localhost:7000/api
```

## Dependencies to Remove

Remove EmailJS from your frontend since it's now handled by the backend:

```bash
npm uninstall @emailjs/browser
```

## Backend Configuration

### Using User Secrets (Recommended - More Secure)

Your project is already configured to use user secrets! Set your sensitive values using these commands:

```bash
# Navigate to the API project directory
cd src/Portfolio.Api

# Set EmailJS settings
dotnet user-secrets set "EmailJsSettings:ServiceId" "your_actual_service_id"
dotnet user-secrets set "EmailJsSettings:TemplateId" "your_actual_template_id"
dotnet user-secrets set "EmailJsSettings:PublicKey" "your_actual_public_key"
dotnet user-secrets set "EmailJsSettings:PrivateKey" "your_actual_private_key"

# Set reCAPTCHA settings
dotnet user-secrets set "RecaptchaSettings:SiteKey" "your_actual_site_key"
dotnet user-secrets set "RecaptchaSettings:SecretKey" "your_actual_secret_key"
```

### View Your Current Secrets

```bash
# List all user secrets
dotnet user-secrets list

# Clear all secrets (if needed)
dotnet user-secrets clear
```

### Why User Secrets?

✅ **Never committed to source control** - Stored locally on your machine  
✅ **Development-friendly** - Easy to manage during development  
✅ **Secure** - Not exposed in your repository  
✅ **Environment-specific** - Each developer can have their own secrets  

### For Production Deployment

User secrets are only for development. For production, use:
- **Azure Key Vault** (recommended for Azure)
- **Environment variables**
- **Docker secrets**
- **Kubernetes secrets**

## Summary of Changes

### What Changed:
- ❌ Removed `import emailjs from '@emailjs/browser'`
- 🔄 Replaced `onSubmit` function to call your API instead of EmailJS
- 🔧 Added API configuration
- 📦 Removed EmailJS dependency

### What Stayed the Same:
- ✅ All your JSX/HTML structure
- ✅ All your styling and CSS
- ✅ reCAPTCHA handling functions
- ✅ Form validation with react-hook-form
- ✅ Toast notifications
- ✅ Loading states and UI interactions

### Security Improvements:
- 🛡️ EmailJS credentials moved from browser to secure server
- 🔒 reCAPTCHA validation happens server-side
- 🚫 No sensitive keys exposed to users anymore

Your frontend will look and behave exactly the same to users, but now it's much more secure! 🎯

## Environment Variables to Remove

You can now remove these environment variables from your frontend since they're no longer exposed to the browser:

- `VITE_APP_EMAILJS_SERVICE_ID`
- `VITE_APP_EMAILJS_TEMPLATE_ID`  
- `VITE_APP_EMAILJS_PUBLIC_KEY`

Keep this one as it's still used for the frontend reCAPTCHA component:
- `VITE_APP_SITE_KEY` (your reCAPTCHA site key)

## Dependencies to Remove

You can remove these packages from your frontend:

```bash
npm uninstall @emailjs/browser
```

## API Configuration

Update your `appsettings.json` with your actual values:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "your-email@gmail.com"
  },
  "RecaptchaSettings": {
    "SiteKey": "your-recaptcha-site-key",
    "SecretKey": "your-recaptcha-secret-key"
  }
}
```

## Running the API

1. Navigate to the Portfolio.Api project directory
2. Run: `dotnet run`
3. The API will be available at `https://localhost:7000` (or the port shown in console)

## CORS Configuration

The API is configured to accept requests from:
- `http://localhost:3000`
- `https://localhost:3000`

Update the CORS policy in `Program.cs` if your frontend runs on different ports.
