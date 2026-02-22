# SQL-Extractor-for-Excel

SQL-Extractor-for-Excel is a practical tool that combines the power of SQL with the flexibility of Excel. It’s a VSTO (Visual Studio Tools for Office) Excel Add-In I created to make my own life easier, and I’m sharing it in case others find it helpful too. 

The idea is simple: make it easy to paste SQL query results directly into Excel ranges and quickly add filters to SQL queries from selected ranges. If, like me, you work a lot with both SQL databases and Excel, this tool bridges the two so they work better together removing time intensive imports from text files also fighting with Excel's data format issues.

---

## What It Does

- **Paste SQL results into Excel**: Run a SQL query and paste the results exactly where you need them in your Excel sheet. 
- **Apply SQL filters from Excel**: Select a range in your sheet, add filter like ('value1', 'value2', ... , 'valueN').
- **Simplifies running queries at once**: Runs queries in the background without need to open slowly new connections. Each query runs independently of each other and can be easily stopped if needed.

---

## Why I Made It

I work a lot with SQL and Excel, and I couldn’t find any tool that let me do what I needed in a simple, seamless way, so I built one. This add-in lets me blend SQL queries with Excel data manipulation without exporting/importing files or jumping between tools. It’s still a work-in-progress, but it’s something I use every day, and it keeps getting better. 

If you’re in the same boat—using SQL and Excel constantly and wishing the two worked together better—this might just be the thing for you.

---

## Current Status

This is an ongoing project that’s still evolving. It’s fully functional for day-to-day workflows but definitely rough around the edges. Expect regular updates as I add features, fix bugs, and generally make it more polished.

---

## How to Use It

1. Build and it will install to your Excel
2. Add databases connections of your need (so far only supports Oracle and MS SQL Server)
3. Start your job, it is so easy

---

## Screenshots

_Screenshots coming soon! I’ll add some pictures or examples of the tool in action so you can see how it works._

---

## Open Source

I’ve decided to make this project open source because I think others might find it useful. Feel free to use it, tinker with it, or suggest improvements. It’s far from perfect, and contributions or feedback are always welcome!

---

## About Me

I’m just someone who works a lot with SQL and Excel and got tired of importing text files. I made this tool to solve a problem I had, and now I’m sharing it in case others want to use it too. If you have any thoughts, feel free to reach out or open an issue. 

---

## License

This project is licensed under the MIT License. Use it however you like!
