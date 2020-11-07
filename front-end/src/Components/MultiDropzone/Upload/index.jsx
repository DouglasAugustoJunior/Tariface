import React, { Component } from "react";
import Dropzone from "react-dropzone";
import { FiCamera, FiAlertCircle, FiDownload } from 'react-icons/fi';
import { DropContainer, UploadMessage } from "./styles";

export default class Upload extends Component {
  renderDragMessage = (isDragActive, isDragReject) => {
    if (!isDragActive) {
      return <UploadMessage><FiCamera size={20}/> Arraste suas fotos aqui...</UploadMessage>;
    }

    if (isDragReject) {
      return <UploadMessage type="error"><FiAlertCircle size={20}/> Foto não suportado</UploadMessage>;
    }

    return <UploadMessage type="success"><FiDownload size={20}/> Solte as fotos aqui</UploadMessage>;
  };

  render() {
    const { onUpload } = this.props;

    return (
      <Dropzone accept="image/*" multiple={false} onDropAccepted={onUpload}>
        {({ getRootProps, getInputProps, isDragActive, isDragReject }) => (
          <DropContainer
            {...getRootProps()}
            isDragActive={isDragActive}
            isDragReject={isDragReject}
          >
            <input {...getInputProps()} />
            {this.renderDragMessage(isDragActive, isDragReject)}
          </DropContainer>
        )}
      </Dropzone>
    );
  }
}
