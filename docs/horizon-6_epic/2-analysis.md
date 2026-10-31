# Horizon 6 Analysis

## Questions

Q: Should records a user logged before this epic is implemented be automatically associated with FH5, or will that data be left as-is (no game linkage on older records)?
A: all records currently on the site should be linked to FH5

Q: When a user switches between games, should that be a global preference (e.g. saved to their profile) or a temporary session-level toggle on the dashboard?
A: Temporary session-level toggle. in the same way a user selects class and event, they should be able to filter by game and see courses and classes assosiated with that game. (classes can also change by game. FH5 has X, S2, S1, A, B, C, D. FH6 has R, S2, S1, A, B, C, D.) 

Q: Will FH6 share any of the same event names as FH5 (e.g. Goliath), or will all of its events be unique to that title?
A: No they will be different. FH6 may have events that share the same name like Goliath, but they are different events with different times

Q: Are there any structural differences between FH5 and FH6 records beyond the available events — for example, different car classes, CPU difficulty tiers, or new fields?
A: Most everything will need to be different. All records, classes, events, and cars are different between games. We can probably just leave cars the same, but the drop down needs to have a search bar on the add record modal so its easy to search through a now larger list of them.

Q: Should users be able to log and view records for both games at the same time, or does switching games replace the current view entirely?
A: It should replace the view. FOr instance, if they are in FH6 and press add ercord, it should only show options for FH6 events and classes and no option to change games within the modal. The game select should be more prominent than the event and class selections on the dashboard.

Q: Who will be responsible for adding new courses to the database — only admins/developers, or should any authenticated user be able to suggest or submit events?
A: Admins. I'll add an admin page at a different time, but for now, courses (no longer called events) should be stored in the database and only added through database/api interaction by me

## Feature Suggestions

Game Switcher UI
- A persistent control on the dashboard (e.g. a toggle or tab) that lets the user switch between FH5 and FH6, filtering all records and events to the selected game.
Approve: true

Course Entity & Management Endpoints
- Store courses in the database with at minimum a name and an associated game. Expose API endpoints to create, read, update, and soft-delete courses, so new events can be added without touching the database directly.
Approve: true

Game-Aware Analysis
- Scope the analysis cards (fastest car, most consistent, etc.) to the currently selected game so that FH5 and FH6 records are never mixed in the same analysis view.
Approve: true

Course Metadata
- Extend the course record with additional descriptive fields such as event type (sprint vs. circuit) or approximate route length to give users more context when browsing records.
Approve: false

## Follow up Questions

Q: Since all existing records need to be retroactively linked to FH5, should a one-time database migration script be part of this epic's scope, or will you handle that manually outside of the implementation work?
A: A migration script is fine. Just make the value for the new column "FH5" as well as anything else that needs the game column. If it's to be a varchar column type, add more than 3 characters incase i use a longer name in the future. in fact, i suppose if they are linked to courses, then the course may be the only thing that needs the game column. up to you

Q: Should car classes (e.g. X vs R) be stored in the database per game, or hardcoded on the frontend per game? Given that courses are now database-driven, it seems worth deciding whether classes follow the same pattern.
A: Hardcoded is fine

Q: Should the Game itself be a database entity (a `Games` table with `game_id`, `name`, etc.), or is it sufficient to represent the game as a simple string/enum column on the `Course` table? A Games table would make the design more extensible but adds complexity.
A:I dont think there's a need for a separate entity at this point

Q: When a user adds a record, the course dropdown should be populated from the database filtered by the selected game — is that correct, or should records still store a free-text event name as they do today?
A: It should be a dropdown filtered by the currently selected game yes

Q: Should the game switcher default to a specific game on load (e.g. always start on FH5), or should it remember the last selected game for the duration of the session?
A: it should default to Forza Horizon 6 (FH6) and I will manually change it if a new game releases

Q: Since cars are shared across games, can a car registered in a user's garage be used in a record for either game? Or is there any intent to eventually restrict which cars are available per game?
A: I have not decided yet. I'll be adding new cars later, so I'll fix that issue when i get there
