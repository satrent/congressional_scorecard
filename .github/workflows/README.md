# Workflows

## Deploy API to ECR (`deploy-api-ecr.yml`)

Builds the Scorecard API Docker image and pushes it to **Amazon ECR** so you can run it with **AWS App Runner**.

### One-time AWS setup

1. **Create an ECR repository** (if you don’t have one):
   - In AWS Console: ECR → Create repository → name e.g. `congressional-scorecard-api`.
   - Or CLI: `aws ecr create-repository --repository-name congressional-scorecard-api --region us-east-1`

2. **Create an IAM user** (or use an existing one) with permissions to push to ECR, e.g.:
   - `ecr:GetAuthorizationToken`
   - `ecr:BatchCheckLayerAvailability`, `ecr:GetDownloadUrlForLayer`, `ecr:BatchGetImage`
   - `ecr:PutImage`, `ecr:InitiateLayerUpload`, `ecr:UploadLayerPart`, `ecr:CompleteLayerUpload`

### GitHub repo setup

1. **Secrets** (Settings → Secrets and variables → Actions):
   - `AWS_ACCESS_KEY_ID` – IAM access key for the user above
   - `AWS_SECRET_ACCESS_KEY` – IAM secret key

2. **Repository name** (optional): To use a different ECR repository name, set the `ECR_REPOSITORY` variable in the workflow file or in repo Variables.

### Triggers

- **Push** to `main` when files under `api/ScorecardAPI/` change
- **Manual**: Actions → Deploy API to ECR → Run workflow

After a run, use the new image URI in App Runner (e.g. `{account}.dkr.ecr.us-east-1.amazonaws.com/congressional-scorecard-api:latest`).
