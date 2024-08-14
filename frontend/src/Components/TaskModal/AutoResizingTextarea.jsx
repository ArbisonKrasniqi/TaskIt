import { useState, useRef, useEffect } from "react";

const AutoResizingTextarea = () => {
  const [description, setDescription] = useState("");
  const textareaRef = useRef(null);

  // Auto-resize the textarea based on its content
  useEffect(() => {
    if (textareaRef.current) {
      textareaRef.current.style.height = "auto"; // Reset the height
      textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`; // Set to scrollHeight
    }
  }, [description]); // Effect runs every time description changes

  return (
    <textarea
      ref={textareaRef}
      value={description}
      onChange={(e) => setDescription(e.target.value)}
      placeholder="Add a more detailed description..."
      className="bg-gray-700 bg-opacity-50 p-2 rounded-md resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 overflow-hidden"
    />
  );
};

export default AutoResizingTextarea;
