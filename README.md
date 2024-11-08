# Shelf_Display

**Developed by Maor Moav**

A WebGL Unity project that displays product information on a 3D shelf, with data dynamically fetched from a server. Users can view products on a virtual shelf, modify their details (name and price), and submit changes. The project is designed to be compatible with both desktop and mobile browsers.

## Project Overview

Shelf_Display is a WebGL application created in Unity that demonstrates a 3D product display. Products are fetched from a remote server and displayed on a shelf, allowing users to interact with the product data. This project showcases core skills in Unity WebGL, server data fetching, and responsive UI design.

### Key Features

- **Dynamic Product Display**: Fetches product data from a server and displays between 1 to 3 products on a 3D shelf.
- **User Interaction**: Users can select a product, update its name and price, and submit changes.
- **Feedback System**: Provides visual feedback for successful or unsuccessful submissions.
- **Responsive Design**: Designed to work on both desktop and mobile browsers.

## Running the Project

To run this project locally:

 **Clone the repository**:
   ```bash
   git clone <repository_url>
## Running the Project

### Open the Project in Unity
- It’s recommended to use **Unity version 2020.3** or higher.

### Build for WebGL
1. Go to **File > Build Settings**.
2. Select **WebGL** as the target platform.
3. Click **Build and Run** to create the WebGL build and test it in a web browser.

## External Assets

- **Textures**: Product textures are sourced from [Poly Haven](https://polyhaven.com/).
- **TextMeshPro**: Used for displaying text within the Unity project.

## Code Structure and Design

### ProductsManager.cs

The `ProductsManager` script is the main controller for managing product data, interactions, and UI updates. It handles:

- **Fetching product data from the server**.
- **Populating the 3D shelf** with product names, prices, and descriptions.
- **Handling user interactions**, allowing users to modify product details and providing feedback on success or failure.

### Key Design Decisions

- **Server-Driven Product Data**: The application dynamically retrieves product data from a server, making it flexible and easy to update.
- **UI Feedback**: Uses color-coded feedback to improve user experience, providing clear messages for successful or failed actions.
- **Responsive Layout**: The canvas is configured to scale with screen size, ensuring compatibility across different devices and screen resolutions.
