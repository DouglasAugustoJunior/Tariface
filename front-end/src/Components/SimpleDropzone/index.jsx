import React, { useCallback, useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { FiCamera } from 'react-icons/fi';
import './styles.css';

const SimpleDropzone = ({ onFileUpload }) => {
  const [selectedFileUrl, setSelectedFileUrl] = useState('')

  const onDrop = useCallback(acceptedFiles => {
    const file = acceptedFiles[0];

    const fileUrl = URL.createObjectURL(file);

    setSelectedFileUrl(fileUrl);

    onFileUpload(file);
  }, [onFileUpload]);

  const { getRootProps, getInputProps } = useDropzone({onDrop, accept: 'image/*', multiple: false});

  return (
    <div className="simple-dropzone" {...getRootProps()}>
      <input {...getInputProps()} accept="image/*" />
      {
        selectedFileUrl ? 
        <img src={selectedFileUrl} alt="Point thumbnail" /> : 
        (<p>
          <FiCamera />
        </p>)
      }
    </div>
  );
}

export default SimpleDropzone;