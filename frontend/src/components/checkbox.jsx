import React from "react";

const Checkbox = ({ id, label, isSelected, onCheckboxChange }) => {
  console.log(`Rendering checkbox component; init: ${isSelected}`);

  return (
    <div className="custom-control custom-switch">
      <input
        type="checkbox"
        id={id}
        checked={isSelected}
        onChange={onCheckboxChange}
        className="custom-control-input"
      />
      <label htmlFor={id} className="custom-control-label">
        {label}
      </label>
    </div>
  );
};
export default Checkbox;
