# Screenshot Capture Notes

This folder contains README-visible placeholder images and is reserved for final portfolio screenshots.

Current placeholders:

- `dashboard-placeholder.svg`
- `customer-registration-placeholder.svg`
- `loan-application-placeholder.svg`
- `eligibility-result-placeholder.svg`
- `audit-trail-placeholder.svg`

Recommended captures:

- `dashboard.png`: operations dashboard first screen.
- `customer-registration.png`: customer registration form with validation-ready layout.
- `loan-application.png`: loan application submission form.
- `eligibility-result.png`: rule result and decision explanation.
- `audit-trail.png`: traceability view filtered by entity.

Suggested local flow:

```powershell
docker compose --profile services --profile frontend up -d --build
```

Open `http://localhost:4200`, capture the screens above, and keep the data synthetic. After real captures are available, either replace the README image links with the PNG files or overwrite the placeholder SVGs with final portfolio images.
