# Congressional Scorecard – Agent Context

Use this when working on this repo so suggestions align with the product and stack.

## What We're Building

A **Congressional Scorecard** site that lets people see their **congress member's record on farm animal advocacy issues**. Constituents can look up their representative and understand how they vote and act on animal welfare and related legislation.

## Architecture

| Layer       | Technology           | Notes                                              |
|------------|----------------------|----------------------------------------------------|
| **Front end** | Angular              | SPA for browsing members, districts, and scorecards |
| **Back end**  | .NET (ASP.NET Core)  | API and business logic; containerized for deploy   |
| **Hosting**   | AWS App Runner       | Runs the containerized .NET API                    |
| **Database**  | Amazon DynamoDB      | Congress members, voting record, advocacy data     |

## Data (DynamoDB)

- **Congress members** – Identity, district, party, contact, photo, etc.
- **Voting record** – Bills, votes (yea/nay/absent), and how they relate to farm animal / advocacy.
- **Advocacy information** – Scores, summaries, and advocacy-specific metadata for members/votes.

## Repo Layout

- **`api/ScorecardAPI/`** – .NET Web API (Dockerfile for App Runner).
- **`ui/`** – Angular app.

When editing code or docs, keep this stack and purpose in mind so recommendations align with the solution.
