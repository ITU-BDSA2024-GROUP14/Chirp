# Chirp

## Issue

### Issue title convention

- [ week.task.subtask.subsubtask ] As a _role_, I want _goal_ [, so that *benefit*].
- [ ] As a role, I want goal, so that.
- `[  ] As a , I want , so that.`

### Issue template

```markdown
### Acceptance criteria:

- [ ] Criteria 1
- [ ]

### Description:

### Refrences:

- [text](link)
- []()
```

## Commands

### Migration

```bash
cd src
dotnet ef migrations add BeakDBSchema --project Chirp.Infrastructure --startup-project Chirp.Web
```

