# SauceDemo E2E Test Suite

[![CI](https://github.com/Lucj0/SauceDemoPlaywright/actions/workflows/ci.yml/badge.svg)](https://github.com/Lucj0/SauceDemoPlaywright/actions/workflows/ci.yml)

End-to-end browser automation for the [SauceDemo](https://www.saucedemo.com) web storefront, built with Playwright and NUnit in C#. The suite covers the full purchase journey and a range of negative and edge cases, organized with the Page Object Model and run in CI on every push.

## Tech stack

- **C# / .NET 10**
- **Playwright** — browser automation (Chromium, Firefox, WebKit)
- **NUnit** — test framework
- **GitHub Actions** — CI

## Testing approach

Tests are structured with the **Page Object Model**: each page of the application (login, inventory, cart, checkout info, checkout overview) has its own class holding that page's locators and actions. Tests call intent-level methods (`LoginAsync`, `AddItemAsync`, `GoToCheckoutAsync`) rather than raw selectors, so tests read as user journeys and a UI change requires editing one page object rather than every test.

Locators target `data-test` attributes wherever available, since these are purpose-built for automation and resilient to styling or layout changes.

## Coverage

- **Happy path** — full purchase flow: login → add to cart → checkout → order confirmation
- **Authentication** — valid login, invalid credentials, and locked-out user (asserting the specific lockout error)
- **Cart operations** — adding items, removing to an empty cart, targeted removal (removing one of several items leaves the correct item), and multi-item cart badge count
- **Sorting** — verifies price low-to-high ordering by extracting all prices and asserting ascending order
- **Defect documentation** — tests that reproduce known bugs in SauceDemo's intentionally broken `error_user` account: the last-name field fails to persist, and checkout cannot be completed

### Known limitations

Some SauceDemo test accounts (`problem_user`, `visual_user`) exhibit purely visual defects — wrong images, misplaced elements — that functional automation does not catch. Detecting these requires visual regression testing (e.g. Applitools), which is outside this suite's scope.

## Running the tests

```bash
# Restore and build
dotnet restore
dotnet build

# Install browsers (first time only)
pwsh bin/Debug/net10.0/playwright.ps1 install

# Run all tests (headless)
dotnet test

# Run with a visible browser
dotnet test -- Playwright.LaunchOptions.Headless=false
```