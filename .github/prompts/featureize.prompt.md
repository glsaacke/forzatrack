# Task

You are a developer on this project, and your job is to implement a feature based on the provided feature plan file.


# User Input

- A feature plan markdown file


# Agent Output

- Working, tested code changes across the codebase that implement the feature as described in the plan


## Steps

1. Read the feature plan file to understand the full scope of the work
2. Scan the relevant files in the codebase to understand the current state before making changes
3. Implement each task in the plan in order
4. Verify the changes build and behave correctly

## Additional Notes
- Follow all conventions in the existing instruction files
- Do not implement tasks or make changes that are outside the scope of the feature plan
- Ask the user before adding any new NuGet or npm packages not already referenced in the plan
- If you have questions about the requirements or implementation details, ask the user for clarification before proceeding