// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ICardActionParameters } from './ICardActionParameters';
import { IOnCardSelectionActionParameters } from './IOnCardSelectionActionParameters';

/**
 * Sharepoint SelectMediaActionParameters object for select media action
 */
export class SelectMediaActionParameters implements ICardActionParameters, IOnCardSelectionActionParameters{
    private mediaType: SelectMediaActionParameters.MediaType;
    private allowMultipleCapture: boolean;
    private maxSizePerFile?: number;
    private supportedFileFormats?: string[];
    /**
     * Initializes a new instance of the SelectMediaActionParameters class
     */
    public SelectMediaActionParameters(){
        // Do nothing
    }

    /**
     * Sets the media type of type string
     */
    public set MediaType(mediaType: SelectMediaActionParameters.MediaType){
        this.mediaType = mediaType;
    }

    /**
     * Gets the media type of type string
     */
    public get MediaType(): SelectMediaActionParameters.MediaType {
        return this.mediaType;
    }

    /**
     * Sets if multiple files can be selected of type boolean
     */
    public set AllowMultipleCapture(allowMultipleCapture: boolean){
        this.allowMultipleCapture = allowMultipleCapture;
    }

    /**
     * Gets if multiple files can be selected of type boolean
     */
    public get AllowMultipleCapture(): boolean {
        return this.allowMultipleCapture;
    }

    /**
     * Sets the max size per file of type number
     */
    public set MaxSizePerFile(maxSizePerFile: number){
        this.maxSizePerFile = maxSizePerFile;
    }

    /**
     * Gets the max size per file  of type number
     */
    public get MaxSizePerFile(): number {
        return this.maxSizePerFile;
    }

    /**
     * Sets the supported file formats of type string[]
     */
    public set SupportedFileFormats(supportedFileFormats: string[]){
        this.supportedFileFormats = supportedFileFormats;
    }

    /**
     * Gets the supported file formats of type string[]
     */
    public get SupportedFileFormats(): string[] {
        return this.supportedFileFormats;
    }
}

export namespace SelectMediaActionParameters{
    export enum MediaType {
        Image = 1,
        Audio = 4,
        Document = 8
    }
}