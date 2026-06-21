# Screenshot Capture Notes

This folder is reserved for portfolio screenshots.

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

Open `http://localhost:4200`, capture the screens above, and keep the data synthetic.
