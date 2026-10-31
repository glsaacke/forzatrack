# Horizon 6 Scope

## Proposed Features

Course Entity & API
- Add a `Course` table to the database with fields for name and game (varchar, e.g. "FH5", "FH6"). Implement full CRUD API endpoints so courses can be managed through the API without direct database access.
Status:

Migrate Existing Events to Courses
- Write a one-time migration script that inserts all currently hardcoded FH5 events (Goliath, Colossus, Gauntlet, Titan, Marathon, Vulcan Sprint) as `Course` rows with game = "FH5", and updates all existing records to reference the correct course.
Status:

Link Records to Courses
- Replace the free-text `Event` string on the `Record` model with a foreign key reference to the `Course` table, so records are always tied to a specific database-managed course.
Status:

Game Switcher on Dashboard
- Add a prominent game selector to the dashboard (above the course and class filters) that defaults to FH6 on load. Switching games replaces the current record view and filters all data — records, course dropdown, and class options — to the selected game.
Status:

Game-Scoped Course & Class Filters
- Update the course dropdown and class filter on the dashboard and Add Record modal to be dynamically populated based on the selected game. Courses load from the API filtered by game; classes are hardcoded per game (FH5: X, S2, S1, A, B, C, D / FH6: R, S2, S1, A, B, C, D).
Status:

Searchable Car Dropdown in Add Record Modal
- Replace the plain car select element in the Add Record modal with a searchable dropdown so users can easily find a car from the growing list.
Status:

Game-Aware Analysis
- Scope the analysis section (fastest car, fastest average, most consistent, most used) to only include records belonging to the currently selected game, preventing FH5 and FH6 data from mixing.
Status:
