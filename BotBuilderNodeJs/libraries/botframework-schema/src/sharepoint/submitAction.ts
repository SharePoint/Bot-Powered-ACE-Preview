// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { Action } from './action';
import { ConfirmationDialog } from './confirmationDialog';

/**
 * Sharepoint action.submit 
 */
export class SubmitAction extends Action {
    private confirmationDialog: ConfirmationDialog;

    /**
     * Initializes a new instance of the SubmitAction class
     */
    public SubmitAction() {
        // Do nothing
    }

    /**
     * Sets confirmation dialog property of type ConfirmationDialog
     */
    public set ConfirmationDialog (confirmationDialog: ConfirmationDialog) {
        this.confirmationDialog = confirmationDialog;
    }

    /**
     * Gets confirmation dialog property of type ConfirmationDialog
     */
    public get ConfirmationDialog(): ConfirmationDialog {
        return this.confirmationDialog; 
    }
}