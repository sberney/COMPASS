# Development Goals & Targets

This file describes some goals.


# sberney

## Near-Term

### Refactor until I understand it

- Set up CI pipeline
  - Rationale: only sane way to verify whether the project is currently working, or whether I/someone else has done something odd locally.
- Set up unit test framework in new solution project
  - Rationale: I am too lazy to try to understand the entire program and run it in my head.
- Create new folder or new project to contain interfaces and business logic code ("CORE"?)
  - Rationale: I will not work on code that lives primarily in views. I need the code encapsulated in classes, interfaces, etc which can be examined in isolation from view logic.
- Set up DI framework
  - Rationale: Ultimately, with the way I write code, I will need one of these if I successfully move the code into classes and interfaces in a library. This is my preferred development style.

### Get it running, analyze, and make changes

- Get the project to build
- Get dependencies like Ghostscript installed
- Get the project to boot up and run
- Understand file loading and import flows
- Examine the possibilities for adding bulk import

## Long-Term/Wishlist

- Multiplatform support
- vscode support & github code sandbox support (see multiplatform support)
- github action support - does github actions work with windows-only vs solutions? it does of course. I know this.
- Button hover colors -- more reactive UX to indicate positional mouse awareness or whatever.
