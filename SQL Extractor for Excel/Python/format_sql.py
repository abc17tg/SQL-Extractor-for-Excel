# # format_sql.py
# import sys
# from sqlfluff.api import fix, APIParsingError
# from sqlfluff.core.config import FluffConfig

# # print("Debugging start...")
# # print(f"Python Version: {sys.version}")
# # print(f"Command-line arguments: {sys.argv}")

# try:
#     # Read input SQL
#     sql = sys.stdin.read().strip()
#     # print(f"\n\nReceived SQL:\n{sql}")

#     if not sql:
#         raise ValueError("No SQL input received.")
        
#     formatted_sql = fix(sql)

#     # # Debugging after fix
#     # print("\n\nAfter fix:\n")
    
#     # Ensure output is flushed properly
#     sys.stdout.write(formatted_sql)
#     sys.stdout.flush()

# except Exception as e:
#     print(f"Full Error: {repr(e)}")
#     sys.exit(1)

# format_sql.py
import sys
from sqlfluff.api import fix, APIParsingError
from sqlfluff.core.config import FluffConfig

def main():
    try:
        # Read dialect from command-line arguments
        if len(sys.argv) < 2:
            dialect = "oracle"  # Default dialect
        else:
            dialect = sys.argv[1].lower()
            if dialect not in ["oracle", "tsql"]:
                raise ValueError(f"Unsupported dialect: {dialect}")

        # Read input SQL
        sql = sys.stdin.read().strip()

        if not sql:
            raise ValueError("No SQL input received.")

        # Configure SQLFluff with the specified dialect
        config = FluffConfig(overrides={"dialect": dialect})
        
        # Format the SQL
        formatted_sql = fix(sql, config=config)

        # Output the formatted SQL
        sys.stdout.write(formatted_sql)
        sys.stdout.flush()

    except Exception as e:
        print(f"Full Error: {repr(e)}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    main()
