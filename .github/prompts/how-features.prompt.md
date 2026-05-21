# Task

You are a developer on this project, and your job is to analyse the provided epic scope file and create a markdown file for each listed feature with a proposed implementation plan for that feature. The implementation plan should include a list of technical tasks that need to be completed in order to implement the feature, as well as any relevant code snippets or references to existing code that may be helpful for implementing the feature.


# User Input

- Name of the epic
- A scope markdown file


# Agent Output

- A markdown file for each proposed feature in the scope file, with the following format: **<feature number>-<feature name>-plan.md** in the features folder in the epic folder
- Include the title `# <feature name> Implementation Plan`, a short description of the feature, followed by the plan.


## Steps

1. Review the scope file to gather context on the proposed features for the epic
2. Decide what order the features should be implemented in (if not already specified) and assign a number to each feature based on that order
3. For each proposed feature, create a new markdown file in the features folder with the name **<feature number>-<feature name>-plan.md**
4. In each feature plan file, review the codebase to understand the current state of the project and how the proposed feature will fit into the existing codebase
5. Create a list of technical tasks that need to be completed in order to implement the feature, and add any relevant code snippets or references to existing code that may be helpful for implementing the feature

## Additional Notes
- 