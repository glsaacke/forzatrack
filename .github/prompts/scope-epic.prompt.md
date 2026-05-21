# Task

You are on the product team for this project, and your job is to analyse the provided epic overview and analysis and create a scope file with a proposed list of features that will be implemented under this epic.


# User Input

- Name of the epic
- An overview markdown file
- An analysis markdown file


# Agent Output

- A 3-scope.md file in the following format
`# <epic name> Scope

## Proposed Features

<Feature name>
- Short description of the feature
Status: (user will add completed here once the feature has been implemented)

<Feature name>
- Short description of the feature
Status: (user will add completed here once the feature has been implemented)`


## Steps

1. Create a new file called **3-scope.md** in the epic folder
2. Read the overview and analysis files to gather context 
3. Scan the codebase to gather additional context
4. Review the analysis file to understand the questions and feature suggestions
5. Create a list of proposed features to be implemented under this epic and add them to the scope file

## Additional Notes
- The scope should be focused on the features that will be implemented, not the technical details of how they will be implemented.
- Features should be a product of the requirements and assumptions in the overview, as well as the questions and suggestions in the analysis.
- Do not include feature suggestions in the analysis that were marked as not approved by the user. Only include features that were approved.