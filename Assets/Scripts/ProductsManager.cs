using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


// Developed by Maor Moav
// This script manages the UI and 3D representation of products, including fetching data from a server, 
// updating product attributes, and providing feedback to users for successful or failed changes.

public class ProductsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown productDropdown; // Dropdown menu for selecting products
    public TMP_InputField productNameInput; // Input field to modify product name
    public TMP_InputField productPriceInput; // Input field to modify product price
    public Button submitButton; // Button to submit changes
    public TextMeshProUGUI feedbackText; // Text element to display feedback messages

    [Header("Product Data")]
    private List<Product> productList = new List<Product>(); // List to store products from the server
    private Product selectedProduct; // Reference to the currently selected product

    [Header("3D Product Objects")]
    public List<GameObject> productObjects; // List of 3D product GameObjects in the scene (Product1, Product2, Product3)

    // Colors for feedback messages: success (green) and error (red)
    private Color successColor = new Color(0.38f, 1f, 0.7f); // #61FFB3
    private Color errorColor = new Color(1f, 0.38f, 0.38f);  // #FF6161

    private void Start()
    {
        // Display a "Loading..." message in the dropdown initially
        productDropdown.options.Clear(); // Clear any default options
        productDropdown.options.Add(new TMP_Dropdown.OptionData("Loading...")); // Placeholder option
        productDropdown.RefreshShownValue();
        
        // Begin fetching product data from the server
        StartCoroutine(FetchProductsFromServer());

        // Set up button and dropdown event listeners
        submitButton.onClick.AddListener(OnSubmitChanges);
        feedbackText.gameObject.SetActive(false); // Initially hide the feedback text
        productDropdown.onValueChanged.AddListener(delegate { OnProductSelected(); });
    }

    // Coroutine to fetch product data from the server
    IEnumerator FetchProductsFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://homework.mocart.io/api/products");
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ProductList products = JsonUtility.FromJson<ProductList>(json);

            // Clear and repopulate productList with the new data
            productList.Clear();
            productList.AddRange(products.products);

            // Activate the appropriate 3D product objects and update their text fields
            ActivateAndPopulateProductObjects();

            // Populate the dropdown with the names of active products
            PopulateDropdown();
        }
        else
        {
            // Handle failed data fetch with an error message
            Debug.LogError("Failed to fetch product data: " + request.error);
            productDropdown.options.Clear();
            productDropdown.options.Add(new TMP_Dropdown.OptionData("Error loading products"));
            productDropdown.RefreshShownValue();
        }
    }

    // Activates only the necessary 3D product objects and sets their details
    private void ActivateAndPopulateProductObjects()
    {
        for (int i = 0; i < productObjects.Count; i++)
        {
            if (i < productList.Count)
            {
                // Activate the product object and update its text fields
                productObjects[i].SetActive(true);
                Transform productTransform = productObjects[i].transform;
                
                // Set name and price text for each product
                productTransform.Find("Name").GetComponent<TextMeshPro>().text = productList[i].name;
                productTransform.Find("Price").GetComponent<TextMeshPro>().text = $"{productList[i].price:F2}";

                // Display description if it exists
                if (!string.IsNullOrEmpty(productList[i].description))
                {
                    productTransform.Find("Description").GetComponent<TextMeshPro>().text = productList[i].description;
                }
            }
            else
            {
                // Deactivate extra product objects
                productObjects[i].SetActive(false);
            }
        }
    }

    // Populates the dropdown menu with the names of active products
    private void PopulateDropdown()
    {
        productDropdown.ClearOptions(); // Clear existing dropdown options

        // Add "Select a product" as the default prompt
        List<string> productNames = new List<string> { "Select a product" };

        // Add the names of products fetched from the server
        foreach (var product in productList)
        {
            productNames.Add(product.name);
        }

        // Apply names to dropdown and refresh the display
        productDropdown.AddOptions(productNames);
        productDropdown.RefreshShownValue();
    }

    // Updates input fields when a new product is selected from the dropdown
    private void OnProductSelected()
    {
        int selectedIndex = productDropdown.value - 1; // Adjust for "Select a product" option
        
        // If a valid product is selected, update the input fields
        if (selectedIndex >= 0 && selectedIndex < productList.Count)
        {
            selectedProduct = productList[selectedIndex]; // Set the selected product
            productNameInput.text = selectedProduct.name; // Display current name
            productPriceInput.text = selectedProduct.price.ToString("F2"); // Display current price
            feedbackText.gameObject.SetActive(false); // Hide previous feedback
        }
    }

    // Called when the Submit button is clicked to apply changes
    private void OnSubmitChanges()
    {
        // Ensure a product is selected before applying changes
        if (productDropdown.value == 0)
        {
            feedbackText.text = "Please select a product first.";
            feedbackText.color = errorColor;
            feedbackText.gameObject.SetActive(true);
            return;
        }

        feedbackText.gameObject.SetActive(false); // Hide feedback on successful selection

        bool updateSuccess = true; // Track if changes are successful

        // Apply name change if a new name is provided
        if (!string.IsNullOrEmpty(productNameInput.text))
        {
            selectedProduct.name = productNameInput.text;

            // Update the 3D object's name display
            productObjects[productDropdown.value - 1].transform.Find("Name").GetComponent<TextMeshPro>().text = selectedProduct.name;

            // Update dropdown option to reflect new name
            productDropdown.options[productDropdown.value].text = selectedProduct.name;
            productDropdown.RefreshShownValue();
        }

        // Apply price change if a valid price is entered
        if (float.TryParse(productPriceInput.text, out float newPrice))
        {
            selectedProduct.price = newPrice;

            // Update the 3D object's price display
            productObjects[productDropdown.value - 1].transform.Find("Price").GetComponent<TextMeshPro>().text = $"{selectedProduct.price:F2}";
        }
        else if (!string.IsNullOrEmpty(productPriceInput.text))
        {
            // Handle invalid price input
            feedbackText.text = "Invalid price format.";
            feedbackText.color = errorColor;
            feedbackText.gameObject.SetActive(true);
            updateSuccess = false;
        }

        // Display success or error feedback based on the outcome
        if (updateSuccess)
        {
            feedbackText.text = "Changes applied successfully.";
            feedbackText.color = successColor;
            feedbackText.gameObject.SetActive(true);
        }
    }
}

// Serializable classes to store product data
[System.Serializable]
public class Product
{
    public string name; // Name of the product
    public string description; // Description of the product
    public float price; // Price of the product
}

[System.Serializable]
public class ProductList
{
    public List<Product> products; // List of products returned by the server
}
