# PowerPages_DotNetWebAPITemplate
Template Web API project to use as Power Pages 'Companion App' using .net 8

This Web API template is a template project that can be used when running OAuth2 implicit flow in Power Pages:
[Use OAuth2 Implicit Flow in Power Pages](https://learn.microsoft.com/en-us/power-pages/security/oauth-implicit-grant-flow?WT.mc_id=M365-MVP-5004644)

# Power Pages Integration with .NET Web API

## Overview

This project provides a template for integrating **Microsoft Power Pages** with a **.NET-based Web API**. While Power Pages is a powerful low-code platform for building external-facing websites using Dataverse, there are limitations when it comes to server-side logic, custom operations, and connecting to external services. This template addresses those gaps by enabling pro-code extensibility using .NET APIs.
---

## Why Use .NET APIs with Power Pages?

Power Pages offer great capabilities with tools like:

- Entity Lists
- Basic & Multi-Step Forms
- Liquid Templates
- JavaScript & jQuery

However, some advanced scenarios require more power than low-code tools can provide. Here are a few reasons to use this template:

- **Custom Backend Logic:** Perform complex actions server-side not possible with JavaScript or Power Automate.
- **Secure External Integrations:** Call external services like SharePoint using app-only tokens.
- **Bypass Web API Limitations:** The Power Pages Web API does not support unbound actions or batch (transactional) requests.
- **Reuse of Dataverse Security Model:** Power Pages automatically apply Dataverse security (table permissions), even with API interactions.

---

## Key Features

- 🔗 **External API Integration:** Connect Power Pages securely with external services through your custom .NET API.
- 🔐 **Token Authentication:** Example includes secure token exchange between Power Pages and your API. 
- 🚀 **Quick Start for Pro Developers:** Ideal for developers looking to extend Power Pages functionality using C#.

---

## Tech Stack
- **.NET 6+ / .NET Core Web API**
- **Microsoft Power Pages**
- **Dataverse (via Power Platform)**
- **OAuth 2.0 or Certificate-based Authentication**
- **Azure AD App Registrations**---

