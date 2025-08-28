# TicketsApp

A cross-platform .NET MAUI application for ticket management, currently under development. This app allows users to view, manage, and interact with support tickets across Android, iOS, and macOS platforms.

## 🚧 Project Status: Work in Progress 🚧

This application is still in development. Core functionality is being implemented, and features may change.

## Tech Stack

- .NET 9 / .NET MAUI
- C# 13
- MVVM Architecture
- Community Toolkit MAUI
- Community Toolkit Markup
- Target Platforms: Desktop and macOS 

## Features (In Progress)

- **User Authentication**: Login functionality for secure access
- **Ticket Management**: View and interact with support tickets
- **Ticket Details**: Detailed view of individual tickets with comments
- **Engineer Interface**: Special views and functionality for engineering staff
- **REST API Communication**:
    - Consistent HTTP client usage with standardized JSON handling
    - Centralized API service classes with clean interfaces
    - Structured HTTP operations (GET, POST, PATCH, DELETE)
    - JSON serialization/deserialization using System.Text.Json
    - Query parameter building for flexible API requests
    - Response parsing with dedicated parser classes
    - Proper error handling for API responses

## Design Principles

- **MVVM Pattern**: Clean separation of concerns using the Model-View-ViewModel pattern
- **Dependency Injection**: Services and components are registered in `MauiProgram.cs`
- **Responsive UI**: Designed to work across the targeted devices


