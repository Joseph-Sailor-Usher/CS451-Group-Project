document.addEventListener("DOMContentLoaded", function () {
     // Function to set the selected color to localStorage
     function setSelectedColor(color) {
          localStorage.setItem("selectedColor", color);
     }

     // Function to get the selected color from localStorage
     function getSelectedColor() {
          return localStorage.getItem("selectedColor");
     }

     // Function to update profile icon color
     function updateSelectedIconColor(colorClass) {
          const selectedIcons = document.querySelectorAll(".selected_icon");
          selectedIcons.forEach(function (icon) {
               icon.classList.remove("red", "orange", "yellow", "green", "blue", "purple", "default");
               icon.classList.add(colorClass);
          });
     }

     // Function to handle icon card click
     function handleIconCardClick(card) {
          const colorClass = card.querySelector(".profile_icon").classList[1];
          setSelectedColor(colorClass);
          updateSelectedIconColor(colorClass);
     }

     // Get all icon cards
     const iconCards = document.querySelectorAll(".icon_card");

     // Loop through each icon card
     iconCards.forEach(function (card) {
          // Add click event listener
          card.addEventListener("click", function () {
               handleIconCardClick(card);
          });
     });

     // Update the profile icon color on page load
     const selectedColor = getSelectedColor();
     if (selectedColor) {
          updateSelectedIconColor(selectedColor);
     }
});
