"use client";

import React from "react";
import {
  Box,
  IconButton,
  Tooltip,
  Collapse,
  Typography,
  useTheme,
} from "@mui/material";
import { IconCode, IconCopy, IconCheck } from "@tabler/icons-react";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
import { vscDarkPlus } from "react-syntax-highlighter/dist/esm/styles/prism";

interface Props {
  children: string;
}

const CodeDialog = ({ children }: Props) => {
  const theme = useTheme();
  const [copied, setCopied] = React.useState(false);
  const [open, setOpen] = React.useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(children);
      setCopied(true);
      setTimeout(() => setCopied(false), 1000);
    } catch (err) {
      console.error("Copy failed", err);
    }
  };

  return (
    <>
      <Collapse in={open}>
        <Box
          sx={{
            backgroundColor: theme.palette.grey[100],
            position: "relative",
            overflowX: "auto",
            px: 2,
            pt: 5,
            pb: 2,
          }}
        >
          <Box sx={{ position: "absolute", top: 8, left: 16 }}>
            <Typography variant="h6" color="text.primary">
              Sample Code
            </Typography>
          </Box>
          <Box
            sx={{
              position: "absolute",
              top: 8,
              right: 12,
              display: "flex",
              gap: 1,
            }}
          >
            <Tooltip title={open ? "Hide Code" : "Show Code"}>
              <IconButton size="small" onClick={() => setOpen(!open)}>
                <IconCode size={18} />
              </IconButton>
            </Tooltip>
            <Tooltip title={copied ? "Copied!" : "Copy Code"}>
              <IconButton size="small" onClick={handleCopy}>
                {copied ? <IconCheck size={18} /> : <IconCopy size={18} />}
              </IconButton>
            </Tooltip>
          </Box>
          <SyntaxHighlighter language="tsx" style={vscDarkPlus}>
            {children}
          </SyntaxHighlighter>
        </Box>
      </Collapse>
      {!open && (
        <Box
          sx={{
            p: 1,
            display: "flex",
            justifyContent: "flex-end",
            backgroundColor: theme.palette.grey[100],
          }}
        >
          <Tooltip title="Show Code">
            <IconButton onClick={() => setOpen(true)}>
              <IconCode size={18} />
            </IconButton>
          </Tooltip>
          <Tooltip title={copied ? "Copied!" : "Copy Code"}>
            <IconButton onClick={handleCopy}>
              {copied ? <IconCheck size={18} /> : <IconCopy size={18} />}
            </IconButton>
          </Tooltip>
        </Box>
      )}
    </>
  );
};

export default CodeDialog;
