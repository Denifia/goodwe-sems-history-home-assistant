## Goodwe Sems History Home Assistant

Downloads daily import, export and load stats from the sems portal for a given day and imports them into long and short term statistics tables in Home Assistant.

> __Use at your own risk__. This app was built for me and may cause issues your unique setup.

> __BUG!__ This app nicely imports the data in the date range you specify but every day after that has a weird spike in the 12-1am time slot.

### Requirements

- Your Sems username, password and plant id
- Home Assistant using sqlite
- Dotnet 6 runtime installed

### Usage

> __STOP HOME ASSISTANT BEFORE PROCEEDING__ because if you don't there is a fair chance this'll corrupt your database.

```bash
.\sems-history-importer record \
  --username your-sems-username \
  --password your-sems-password \
  --plant your-sems-plant-id \
  --sqlite path/to/home-assistant_v2.db \
  --start yyyy-MM-dd \
  --end yyyy-MM-dd \
  --timezone hh:mm
```

- `start` to `end` is the date range of data you want to pull from Sems, inclusive.
- All Sems HomeKit import/export stats in Home Assistant before and including `end` will be deleted.

### How it works

1. Logs into Sems
2. Finds your HomeKit serial number
3. Looks in Home Assistant for import/export sensors with that serial number
4. Downloads HomeKit data for your date range
5. Remove HomeKit import/export statistics in Home Assistant on or before `end` date
6. Import the new data
7. Recalculate _all_ HomeKit import/export sums (including stats outside of your date range)

### Credits

This is a spin-off project from [goodwe-sems-home-assistant](https://github.com/TimSoethout/goodwe-sems-home-assistant).