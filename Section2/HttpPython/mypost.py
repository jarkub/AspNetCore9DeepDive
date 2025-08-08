import json
import requests
from requests.exceptions import JSONDecodeError, RequestException, HTTPError

def putit():
    return requests.put(url, json=payload)


def postit():
    return requests.post(url, json=payload)


def getit():
    return requests.get(url, data=payload)


def printit(response):
    response.raise_for_status()  # Raise an exception for HTTP errors (4xx or 5xx)
    print(f"response=[{response}] is valid json? [{is_json(response.content)}]")
    print("Status Code:", response.status_code)
    if is_json(response.content):
        pretty_json = json.dumps(response.json(), indent=4)
        print("Response Content (JSON):", pretty_json)
    else:
        print(response.text)

    return


def is_json(myjson_string):
    """
    Checks if a given string is valid JSON.

    Args:
        myjson_string (str): The string to be checked.

    Returns:
        bool: True if the string is valid JSON, False otherwise.
    """
    try:
        json.loads(myjson_string)
        return True
    except json.JSONDecodeError:
        return False

url = "http://localhost:5007/"
payload = {}
headers = {
    "User-Agent": "MyCustomApp/1.0",
    "Authorization": "Bearer your_access_token",
    "Content-Type": "application/json",
}
# url = "https://httpbin.org/post"  # A common endpoint for testing POST requests
# payload = {"key1": "value1", "key2": "value2"}

# url = "http://localhost:5007/employees"
# payload = {
#     "salary": "100.01",
#     "name": "Jared",
#     "position": "MVP",
# }

# url = "http://localhost:5007/myclass"
# payload = {"id": 1, "name": "Jared", "tits":"yes"}

if __name__ == "__main__":
    try:
        response = requests.delete("http://localhost:5007/fakeauth", headers=headers, json=payload)
        # response = getit()
        # response = postit()
        # printit(response)

        # payload["position"] = "Boss Bitch 2"

        # response = putit()
        printit(response)

    # All exceptions that Requests explicitly raises inherit from requests.exceptions.RequestException.
    except JSONDecodeError as e:
        print(f"Error decoding JSON: {e}")
        print(f"Response text: {response.text}")
        print(
            f"""An error occurred: 
              Type=[{type(e)}] 
              Response=[{e.response}]"""
        )
    except HTTPError as e:
        print(f"HTTP Error: {e}")
        print(f"Response status code: {response.status_code}")
        print(f"Response text: {response.text}")
    except RequestException as e:
        print(f"An error occurred during the request: {e}")
        print(
            f"""Type=[{type(e)}] 
                Message=[{e}]"""
        )
    except Exception as e:
        print(f"An unknown exception occurred {e}")
