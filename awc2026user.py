
import requests
import csv

CLIENT_ID = "a8d2b240905ef16258075005cacca8db"
TOPIC_ID = 2245806

users = set()
offset = 0
limit = 100

while True:
    url = f"https://api.myanimelist.net/v2/forum/topic/{TOPIC_ID}?limit={limit}&offset={offset}"
    headers = {
        "X-MAL-CLIENT-ID": CLIENT_ID
    }

    response = requests.get(url, headers=headers)
    response.raise_for_status()
    data = response.json()

    # Collect usernames
    for post in data["data"]["posts"]:
        username = post["created_by"]["name"]
        users.add(username)

    # Pagination
    paging = data.get("paging", {})
    if "next" in paging:
        offset += limit
    else:
        break

# Write to CSV
with open("users.csv", "w", newline="", encoding="utf-8") as f:
    writer = csv.writer(f)
    writer.writerow(["username"])
    for user in sorted(users):
        writer.writerow([user])

print("Saved", len(users), "users to users.csv")

