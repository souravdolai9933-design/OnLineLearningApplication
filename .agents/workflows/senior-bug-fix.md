---
description: A strict bug-fixing workflow that identifies the root cause, plans and gets approval for minimal changes, preserves existing logic, thoroughly tests the fix, checks regressions, reviews changes, and reports results transparently.
---

# Senior Bug Fix & Issue Resolution Workflow

## Objective

Fix the user's reported issue with the smallest possible change while preserving all existing functionality, business logic, architecture, and behavior.

Do not add logic based on assumptions or personal interpretation.

---

## PHASE 1 — UNDERSTAND THE ISSUE

1. Read the user's complete request carefully.
2. Identify exactly what is broken.
3. Identify the expected behavior.
4. Identify the actual behavior.
5. Determine the scope of the requested change.

Do not modify any files during this phase.

If the requirement is unclear, ask for clarification instead of guessing.

---

## PHASE 2 — INSPECT THE EXISTING CODE

Before making any change:

1. Inspect the relevant files.
2. Understand the existing implementation.
3. Find where the reported functionality starts.
4. Trace the execution flow.
5. Identify dependencies between components.

For an ASP.NET MVC application, inspect the flow when applicable:

Frontend
→ JavaScript/jQuery
→ AJAX/HTTP Request
→ Controller
→ Service
→ Repository
→ Stored Procedure
→ Database
→ Response
→ Frontend

Do not assume how the system works.
Verify it from the actual code.

---

## PHASE 3 — FIND THE ROOT CAUSE

Determine the actual root cause using evidence.

Check:

- Existing code
- Method calls
- Parameters
- Data flow
- API requests/responses
- Browser console
- Network requests
- C# exceptions
- SQL queries
- Stored Procedures
- Database results
- Build/test errors

Clearly separate:

ROOT CAUSE
from
SYMPTOM.

Do not fix only the symptom if the actual root cause can be identified.

Do not create additional logic to compensate for an unknown cause.

---

## PHASE 4 — CHECK CHANGE SCOPE

Before proposing the fix, determine:

1. Which file(s) actually need to change?
2. Which method(s) need to change?
3. Does the fix require frontend changes?
4. Does the fix require backend changes?
5. Does the fix require SQL/Stored Procedure changes?
6. Could the fix affect existing functionality?

Do not modify a component simply because it is related to the feature.

Only modify what is necessary.

---

## PHASE 5 — CREATE A FIX PLAN

Before implementation, provide:

### Root Cause
Explain exactly why the issue occurs.

### Proposed Fix
Explain exactly what needs to change.

### Files To Change
List only the files that need modification.

### Files Not To Change
Mention important files/components that will remain untouched.

### Risk
Explain possible side effects or regression risks.

### Testing Plan
Explain how the fix will be verified.

For non-trivial changes, STOP and wait for user approval.

---

## PHASE 6 — IMPLEMENT

After approval:

1. Make only the approved changes.
2. Make the smallest possible change.
3. Preserve existing code structure.
4. Follow existing project conventions.
5. Do not refactor unrelated code.
6. Do not introduce new libraries.
7. Do not create additional features.
8. Do not change business logic unless explicitly required.
9. Do not modify unrelated files.

If you discover that the approved approach requires additional changes:

STOP.

Explain the newly discovered requirement and ask for permission.

---

## PHASE 7 — VERIFY THE IMPLEMENTATION

After implementation, verify the change.

Run the appropriate checks:

### Backend
- Build/compile the application.
- Check C# errors.
- Check relevant exceptions.

### Frontend
- Check JavaScript errors.
- Check browser console.
- Check network requests where applicable.
- Verify the affected UI behavior.

### Database
- Validate SQL syntax.
- Verify Stored Procedure behavior where applicable.
- Check returned data.
- Check joins and filtering.
- Check duplicate rows.
- Check NULL behavior.

### Functional Testing
Verify:

1. The original issue is fixed.
2. Expected behavior works.
3. Important edge cases work.
4. Existing functionality still works.

Never claim that the issue is fixed without verification.

---

## PHASE 8 — REGRESSION CHECK

Check whether the change could affect:

- Existing functionality
- Other controllers
- Other JavaScript functions
- Other API consumers
- Existing Stored Procedure callers
- Database behavior
- Related UI functionality

Do not perform unnecessary broad refactoring.

Only report meaningful regression risks.

---

## PHASE 9 — REVIEW THE FINAL CHANGE

Before finishing:

1. Review the final diff.
2. Check every changed file.
3. Confirm no unrelated changes were introduced.
4. Confirm temporary debugging code is removed.
5. Confirm no unnecessary comments were added.
6. Confirm no unnecessary dependencies were added.
7. Confirm the implementation matches the approved plan.

---

## PHASE 10 — FINAL REPORT

Always provide the following final report:

### 1. Issue
What was reported.

### 2. Root Cause
The actual reason for the problem.

### 3. Fix
Exactly what was changed.

### 4. Files Changed
List every modified file.

### 5. Testing Performed
List the tests/checks that were executed.

### 6. Build Status
PASS / FAIL / NOT RUN

### 7. Functional Verification
PASS / FAIL / PARTIAL

### 8. Regression Check
PASS / FAIL / PARTIAL

### 9. Remaining Issues
Clearly list anything that could not be verified.

### 10. Final Status

FIXED
or
PARTIALLY FIXED
or
NOT FIXED

Never hide failures or limitations.

---

# STRICT RULES

1. Do not invent requirements.
2. Do not add your own business logic.
3. Do not modify unrelated logic.
4. Do not refactor unrelated code.
5. Do not modify additional files without a reason.
6. Do not change C#, JavaScript, SQL, Stored Procedures, frontend, or database logic outside the approved scope.
7. Do not introduce new libraries without approval.
8. Do not create Git commits unless explicitly requested.
9. Do not discard or overwrite existing user changes.
10. Do not claim success without testing.
11. If uncertain, STOP and ask.
12. Preserve existing behavior unless the requested fix requires changing it.

# GOLDEN PRINCIPLE

UNDERSTAND
→ INVESTIGATE
→ IDENTIFY ROOT CAUSE
→ PLAN
→ GET APPROVAL
→ IMPLEMENT MINIMAL FIX
→ TEST
→ VERIFY
→ REVIEW
→ REPORT